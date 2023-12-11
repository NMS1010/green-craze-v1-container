using green_craze_be_v1.Application.Model.OrderCancellationReason;
using green_craze_be_v1.Application.Model.PaymentMethod;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.OrderCancellationReason
{
    public class OrderCancellationReasonSpecification : BaseSpecification<Domain.Entities.OrderCancellationReason>
    {
        public OrderCancellationReasonSpecification(GetOrderCancellationReasonPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                if (request.Status)
                {
                    Criteria = x => x.Status && (x.Name.ToLower().Contains(keyword)
                        || x.Note.ToLower().Contains(keyword));
                }
                else
                {
                    Criteria = x => x.Name.ToLower().Contains(keyword)
                        || x.Note.ToLower().Contains(keyword);
                }
            }
            else
            {
                if (request.Status)
                {
                    Criteria = x => x.Status;
                }
                else
                {
                    Criteria = x => true;
                }
            }

            if (string.IsNullOrEmpty(request.ColumnName))
                request.ColumnName = "CreatedAt";
            AddSorting(request.ColumnName, request.IsSortAscending);
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}