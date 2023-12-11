using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Mail;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Address;
using green_craze_be_v1.Application.Specification.Cart;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Review;
using green_craze_be_v1.Application.Specification.User;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;
using Hangfire;

namespace green_craze_be_v1.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IDateTimeService dateTimeService, ICurrentUserService currentUserService,
            INotificationService notificationService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
            _mailService = mailService;
        }

        private async Task<Order> InitOrder(AppUser user, CreateOrderRequest request)
        {
            var userAddress = user.Addresses.FirstOrDefault(x => x.IsDefault == true)
                    ?? throw new NotFoundException("Cannot find default user's address");

            var delivery = await _unitOfWork.Repository<Delivery>().GetById(request.DeliveryId)
                ?? throw new InvalidRequestException("Unexpected deliveryId");

            var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(request.PaymentMethodId)
                ?? throw new InvalidRequestException("Unexpected paymentMethodId");

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetById(item.VariantId)
                    ?? throw new InvalidRequestException("Unexpected variantId");
                var variantPrice = item.Quantity * variant.TotalPrice;
                totalAmount += variantPrice;
                orderItems.Add(new OrderItem()
                {
                    Quantity = item.Quantity,
                    Variant = variant,
                    UnitPrice = variant.TotalPrice,
                    TotalPrice = variantPrice
                });
            }

            var tax = totalAmount * ORDER_TAX.TAX;
            totalAmount = totalAmount + delivery.Price + tax;

            var transaction = new Transaction()
            {
                PaymentMethod = paymentMethod.Code,
                TotalPay = totalAmount
            };


            var order = new Order()
            {
                User = user,
                Address = userAddress,
                DeliveryMethod = delivery.Name,
                Transaction = transaction,
                OrderItems = orderItems,
                Note = request.Note,
                ShippingCost = delivery.Price,
                Tax = (double)ORDER_TAX.TAX,
                Code = StringUtil.GenerateUniqueCode(),
                Status = ORDER_STATUS.NOT_PROCESSED,
                PaymentStatus = false,
                TotalAmount = totalAmount
            };

            return order;
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

        private async Task UpdateUserCart(List<CreateOrderItemRequest> items, Cart cart)
        {
            foreach (var item in items)
            {
                var cartItem = await _unitOfWork.Repository<CartItem>()
                    .GetEntityWithSpec(new CartItemSpecification(cart.Id, item.VariantId))
                    ?? throw new InvalidRequestException("Unexpected variantId");

                _unitOfWork.Repository<CartItem>().Delete(cartItem);
            }
        }

        private async Task SendOrderMail(AppUser user, Order order)
        {
            var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(user.Id, true));
            var orderDetail = new OrderConfirmationMail()
            {
                Email = address.Email,
                Receiver = address.Receiver,
                Phone = address.Phone,
                Address = $"{address.Street}, {address?.Ward?.Name}, {address?.District?.Name}, {address?.Province?.Name}",
                PaymentMethod = order.Transaction.PaymentMethod,
                TotalPrice = order.TotalAmount
            };

            var req = new CreateMailRequest()
            {
                Email = user.Email,
                Name = user.FirstName + " " + user.LastName,
                Type = MAIL_TYPE.ORDER_CONFIRMATION,
                Title = "Xác nhận đặt hàng",
                OrderConfirmationMail = orderDetail
            };

            _mailService.SendMail(req);
        }

        private async Task NotifyCreateOrder(AppUser user, Order order)
        {
            var notiRequest = new CreateNotificationRequest()
            {
                UserId = user.Id,
                Image = order.OrderItems.FirstOrDefault()?.Variant.Product.Images.FirstOrDefault()?.Image,
                Title = "Đặt hàng thành công",
                Content = $"Đơn hàng #{order.Code} của bạn đã được hệ thống ghi nhận và đang được xử lý",
                Type = NOTIFICATION_TYPE.ORDER,
                Anchor = "/user/order/" + order.Code,
            };
            if (order.Transaction.PaymentMethod != PAYMENT_CODE.PAYPAL)
            {
                await SendOrderMail(user, order);
            }
            else
            {
                notiRequest.Title = "Đơn hàng cần thanh toán";
                notiRequest.Content = $"Đơn hàng #{order.Code} của bạn cần được thanh toán qua Paypal trước khi hệ thống có thể xử lý";
                notiRequest.Anchor = "/checkout/payment/" + order.Code;

                BackgroundJob.Schedule<IBackgroundJobService>(x => x.CancelOrder(order.Id), TimeSpan.FromMinutes(3));
            }

            await _notificationService.CreateOrderNotification(notiRequest);
        }

        public async Task<string> CreateOrder(CreateOrderRequest request)
        {

            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find current user");

            var order = await InitOrder(user, request);

            try
            {
                await _unitOfWork.CreateTransaction();

                await _unitOfWork.Repository<Order>().Insert(order);

                await UpdateProduct(request.Items, order);

                await UpdateUserCart(request.Items, user.Cart);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to create order, an error has occured");
                }

                await _unitOfWork.Commit();
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }

            try
            {
                await NotifyCreateOrder(user, order);

                return order.Code;
            }
            catch
            {
                return order.Code;
            }

        }

        public async Task<PaginatedResult<OrderDto>> GetListOrder(GetOrderPagingRequest request)
        {
            var orders = await _unitOfWork.Repository<Order>().ListAsync(new OrderSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<Order>().CountAsync(new OrderSpecification(request));

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));
                var dto = _mapper.Map<OrderDto>(order);
                dto.Items = await GetOrderItemDto(listOrderItem);
                orderDtos.Add(dto);
            }

            return new PaginatedResult<OrderDto>(orderDtos, request.PageIndex, count, request.PageSize);
        }

        public Task<PaginatedResult<OrderDto>> GetListUserOrder(GetOrderPagingRequest request)
        {
            return GetListOrder(request);
        }

        private async Task<List<OrderItemDto>> GetOrderItemDto(List<OrderItem> listOrderItem)
        {
            var listItems = new List<OrderItemDto>();
            foreach (var oi in listOrderItem)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(oi.Variant.Id))
                ?? throw new NotFoundException("Cannot find varaint item");

                var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(variant.Product.Id))
                    ?? throw new NotFoundException("Cannot find product of variant item");

                var orderItemDto = _mapper.Map<OrderItemDto>(oi);
                orderItemDto.ProductId = product.Id;
                orderItemDto.VariantQuantity = variant.Quantity;
                orderItemDto.VariantName = variant.Name;
                orderItemDto.Sku = /*product.Code + "-" +*/ variant.Sku;
                orderItemDto.ProductName = product.Name;
                orderItemDto.ProductSlug = product.Slug;
                orderItemDto.ProductUnit = product.Unit.Name;
                orderItemDto.ProductImage = product.Images.FirstOrDefault(x => x.IsDefault)?.Image ?? product.Images.FirstOrDefault()?.Image;

                listItems.Add(orderItemDto);
            }
            return listItems;
        }

        public async Task<OrderDto> GetOrder(long id)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(id))
                ?? throw new InvalidRequestException("Unexpected orderId");
            var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));

            var listItems = await GetOrderItemDto(listOrderItem);

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.Items = listItems;

            return orderDto;
        }

        private void ValidateOrderStatus(Order order, string status)
        {
            // Cannot update order while it's not paid by user through PayPal
            if (order.Status == ORDER_STATUS.NOT_PROCESSED
                && status != ORDER_STATUS.CANCELLED
                && order.PaymentStatus == false
                && order.Transaction.PaymentMethod == PAYMENT_CODE.PAYPAL)
            {
                throw new InvalidRequestException("Cannot update this order status, it wasn't paid by user through PayPal");
            }

            //  Cannot cancel order while it's paid
            if (status == ORDER_STATUS.CANCELLED
                && order.PaymentStatus == true)
            {
                throw new InvalidRequestException("Cannot update this order status, it was paid by user before");
            }

            // Cannot update order while it's delivered
            if (order.Status == ORDER_STATUS.DELIVERED)
                throw new InvalidRequestException("Unexpected order status, cannot update order while it's delivered");

            // Cannot update order while it's cancelled
            if (order.Status == ORDER_STATUS.CANCELLED)
                throw new InvalidRequestException("Unexpected order status, order was cancelled");

            // Customer cannot cancel order while it's processing, only admin and staff can do that
            if (order.Status != ORDER_STATUS.NOT_PROCESSED
                && status == ORDER_STATUS.CANCELLED
                && !(_currentUserService.IsInRole(USER_ROLE.ADMIN) || _currentUserService.IsInRole(USER_ROLE.STAFF)))
                throw new InvalidRequestException("Unexpected order status, cannot cancel order while it's processing");
        }

        private async Task HandleChangeStatus(Order order, UpdateOrderRequest request)
        {
            var now = _dateTimeService.Current;
            order.Status = request.Status;
            if (request.Status == ORDER_STATUS.DELIVERED)
            {
                order.PaymentStatus = true;
                if (order.Transaction.PaymentMethod == PAYMENT_CODE.COD)
                    order.Transaction.PaidAt = now;
                order.Transaction.CompletedAt = now;
            }
            else if (request.Status == ORDER_STATUS.CANCELLED)
            {
                if (request.OrderCancellationReasonId.HasValue)
                {
                    var cancellationReason = await _unitOfWork.Repository<OrderCancellationReason>()
                        .GetById(request.OrderCancellationReasonId)
                        ?? throw new InvalidRequestException("Unexpected orderCancellationReasonId");
                    order.CancelReason = cancellationReason;
                }
                else if (!string.IsNullOrEmpty(request.OtherCancellation))
                {
                    order.OtherCancelReason = request.OtherCancellation;
                }

                var orderItems = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));
                await UpdateProduct(orderItems.Select(x => new CreateOrderItemRequest()
                {
                    Quantity = -1 * x.Quantity,
                    VariantId = x.Variant.Id
                }).ToList(), order, DOCKET_TYPE.IMPORT);

            }
            else
            {
                order.CancelReason = null;
                order.OtherCancelReason = null;
            }
        }

        private async Task NotifyUpdateOrder(Order order)
        {
            var orderItem = await _unitOfWork.Repository<OrderItem>().GetEntityWithSpec(new OrderItemSpecification(order.OrderItems.FirstOrDefault().Id, order.Status));

            await _notificationService.CreateOrderNotification(new CreateNotificationRequest()
            {
                UserId = order.User.Id,
                Image = orderItem?.Variant.Product.Images.FirstOrDefault()?.Image,
                Title = "Cập nhật đơn hàng",
                Content = $"Đơn hàng #{order.Code} của bạn đã chuyển sang trạng thái {ORDER_STATUS.OrderStatusSubTitle[order.Status]}",
                Type = NOTIFICATION_TYPE.ORDER,
                Anchor = "/user/order/" + order.Code,
            });
        }

        public async Task<bool> UpdateOrder(UpdateOrderRequest request)
        {

            var spec = new OrderSpecification(request.OrderId);
            if (!string.IsNullOrEmpty(request.UserId))
            {
                spec = new OrderSpecification(request.OrderId, request.UserId);
            }
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec)
                ?? throw new InvalidRequestException("Unexpected orderId");

            ValidateOrderStatus(order, request.Status);

            await HandleChangeStatus(order, request);

            try
            {
                await _unitOfWork.CreateTransaction();

                _unitOfWork.Repository<Order>().Update(order);

                var isSuccess = await _unitOfWork.Save() > 0;

                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to update order, an error has occured");
                }
                await _unitOfWork.Commit();

            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }

            try
            {

                await NotifyUpdateOrder(order);

                return true;
            }
            catch
            {
                return true;
            }

        }

        public async Task<OrderDto> GetOrderByCode(string code, string userId)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(code, userId))
                ?? throw new InvalidRequestException("Unexpected order code");
            var listOrderItem = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));
            var listReview = await _unitOfWork.Repository<Review>().ListAsync(new ReviewSpecification(order.Id));
            var listItems = await GetOrderItemDto(listOrderItem);

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.Items = listItems;
            orderDto.IsReview = listReview.Count == listItems.Count;
            orderDto.ReviewedDate = listReview.Count == listItems.Count ? listReview.Max(x => x.CreatedAt) : null;

            return orderDto;
        }

        private async Task NotifyCompletePaypalOrder(Order order)
        {
            await SendOrderMail(order.User, order);

            var orderItems = await _unitOfWork.Repository<OrderItem>().ListAsync(new OrderItemSpecification(order.Id));

            await _notificationService.CreateOrderNotification(new CreateNotificationRequest()
            {
                UserId = order.User.Id,
                Image = orderItems.FirstOrDefault()?.Variant.Product.Images.FirstOrDefault(x => x.IsDefault)?.Image,
                Title = "Thanh toán thành công",
                Content = $"Đơn hàng #{order.Code} của bạn đã được thanh toán, hệ thống đã ghi nhận và đang được xử lý",
                Type = NOTIFICATION_TYPE.ORDER,
                Anchor = "/user/order/" + order.Code,
            });
        }

        public async Task<bool> CompletePaypalOrder(CompletePaypalOrderRequest request)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(request.OrderId, request.UserId))
            ?? throw new InvalidRequestException("Unexpected order id");

            order.Transaction.PaidAt = _dateTimeService.Current;
            order.Transaction.PaypalOrderId = request.PaypalOrderId;
            order.Transaction.PaypalOrderStatus = request.PaypalOrderStatus;
            order.PaymentStatus = true;
            order.Status = ORDER_STATUS.PROCESSING;
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

            try
            {
                await NotifyCompletePaypalOrder(order);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<OrderDto>> GetTop5OrderLatest()
        {
            List<OrderDto> orderDtos = new();
            var orders = await _unitOfWork.Repository<Order>().ListAsync(new OrderSpecification(5));
            orders.ForEach(order => orderDtos.Add(_mapper.Map<OrderDto>(order)));
            return orderDtos;
        }
    }
}