using green_craze_be_v1.Application.Model.Order;

namespace green_craze_be_v1.Application.Specification.Order
{
    public class OrderSpecification : BaseSpecification<Domain.Entities.Order>
    {
        public OrderSpecification(string status) : base(x => x.Status == status)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(long orderId) : base(x => x.Id == orderId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(long orderId, string userId) : base(x => x.Id == orderId && x.User.Id == userId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(string code, string userId) : base(x => x.Code == code && x.User.Id == userId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(int limit)
        {
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(limit, 0);
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
        }

        public OrderSpecification(DateTime firstDate, DateTime lastDate, string status)
            : base(x => x.CreatedAt >= firstDate && x.CreatedAt <= lastDate && x.Status == status)
        { }

        public OrderSpecification(GetOrderPagingRequest request, bool isPaging = false)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Address);
            AddInclude(x => x.Address.Ward);
            AddInclude(x => x.Address.District);
            AddInclude(x => x.Address.Province);
            AddInclude(x => x.Transaction);
            AddInclude(x => x.CancelReason);
            AddInclude(x => x.OrderItems);
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(request.UserId))
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (!string.IsNullOrEmpty(request.OrderStatus))
                    {
                        Criteria = x => x.User.Id == request.UserId &&
                            x.Code.ToLower().Contains(keyword) && x.Status == request.OrderStatus;
                    }
                    else
                    {
                        Criteria = x => x.User.Id == request.UserId &&
                            x.Code.ToLower().Contains(keyword);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.OrderStatus))
                    {
                        Criteria = x => x.User.Id == request.UserId && x.Status == request.OrderStatus;
                    }
                    else
                    {
                        Criteria = x => x.User.Id == request.UserId;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (!string.IsNullOrEmpty(request.OrderStatus))
                    {
                        Criteria = x => x.Code.ToLower().Contains(keyword)
                            && x.Status == request.OrderStatus;
                    }
                    else
                    {
                        Criteria = x => x.Code.ToLower().Contains(keyword);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.OrderStatus))
                    {
                        Criteria = x => x.Status == request.OrderStatus;
                    }
                    else
                    {
                        Criteria = x => true;
                    }
                }
            }
            if (request.ColumnName.ToLower() == nameof(Domain.Entities.Order.Transaction.PaymentMethod).ToLower())
            {
                if (request.IsSortAscending)
                    AddOrderBy(x => x.Transaction.PaymentMethod);
                else
                    AddOrderByDescending(x => x.Transaction.PaymentMethod);
            }
            else
            {
                if (string.IsNullOrEmpty(request.ColumnName))
                    request.ColumnName = "CreatedAt";
                AddSorting(request.ColumnName, request.IsSortAscending);
            }

            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}