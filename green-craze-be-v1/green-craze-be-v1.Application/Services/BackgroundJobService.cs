using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public BackgroundJobService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        private async Task UpdateProduct(List<CreateOrderItemRequest> Items, Order order, string type = DOCKET_TYPE.EXPORT)
        {
            foreach (var item in Items)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(item.VariantId))
                    ?? throw new InvalidRequestException("Unexpected variantId");
                var product = variant.Product ?? throw new InvalidRequestException("Unexpected variantId");

                var q = item.Quantity * variant.Quantity;

                if (product.ActualInventory < q)
                    throw new Exception("Unexpected quantity");

                var docket = new Docket()
                {
                    Code = StringUtil.GenerateUniqueCode(),
                    Product = product,
                    Order = order,
                    Type = type,
                    Quantity = q,
                };

                await _unitOfWork.Repository<Docket>().Insert(docket);

                product.Quantity -= q;
                product.ActualInventory -= q;
                product.Sold += q;

                _unitOfWork.Repository<Product>().Update(product);
            }
        }

        public async Task CancelOrder(long orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(orderId));

            if (order != null && order.Transaction.PaymentMethod == PAYMENT_CODE.PAYPAL
                && !order.PaymentStatus && order.Status == ORDER_STATUS.NOT_PROCESSED)
            {
                order.Status = ORDER_STATUS.CANCELLED;
                order.OtherCancelReason = "Không thanh toán đúng thời hạn";

                var orderItems = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(orderId));

                await UpdateProduct(orderItems.Select(x => new CreateOrderItemRequest()
                {
                    Quantity = -1 * x.Quantity,
                    VariantId = x.Variant.Id
                }).ToList(), order, DOCKET_TYPE.IMPORT);

                try
                {
                    await _unitOfWork.CreateTransaction();

                    _unitOfWork.Repository<Order>().Update(order);

                    await _unitOfWork.Save();
                    await _unitOfWork.Commit();

                }
                catch
                {
                    await _unitOfWork.Rollback();
                    throw;
                }

                var notiRequest = new CreateNotificationRequest()
                {
                    UserId = order?.User.Id,
                    Image = orderItems?.FirstOrDefault()?.Variant?.Product?.Images.FirstOrDefault()?.Image,
                    Title = "Đơn hàng bị huỷ",
                    Content = $"Đơn hàng #{order.Code} của bạn đã bị huỷ do không thanh toán đúng thời hạn",
                    Type = NOTIFICATION_TYPE.ORDER,
                    Anchor = "/user/order/" + order.Code,
                };

                await _notificationService.CreateOrderNotification(notiRequest);
            }
        }
    }
}
