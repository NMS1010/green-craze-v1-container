using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Specification.Order
{
    public class OrderItemSpecification : BaseSpecification<OrderItem>
    {
        public OrderItemSpecification(long orderId) : base(x => x.Order.Id == orderId)
        {
            AddInclude(x => x.Variant);
            AddInclude(x => x.Variant.Product);
            AddInclude(x => x.Variant.Product.Images);
        }

        public OrderItemSpecification(long orderItemId, string status) : base(x => x.Id == orderItemId && x.Order.Status == status)
        {
            AddInclude(x => x.Variant);
            AddInclude(x => x.Variant.Product);
            AddInclude(x => x.Variant.Product.Images);
        }

        public OrderItemSpecification(long variantId, DateTime firstDate, DateTime lastDate, string status)
            : base(
                  x => x.Variant.Id == variantId
                  && x.Order.CreatedAt >= firstDate
                  && x.Order.CreatedAt <= lastDate
                  && x.Order.Status == status)
        { }
    }
}