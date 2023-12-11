using green_craze_be_v1.Application.Model.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.PaymentMethod
{
    public class PaymentMethodSpecification : BaseSpecification<Domain.Entities.PaymentMethod>
    {
        public PaymentMethodSpecification(GetPaymentMethodPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                if (request.Status)
                    Criteria = x => (x.Name.Contains(keyword) || x.Code.Contains(keyword)) && x.Status == true;
                else
                    Criteria = x => x.Name.Contains(keyword) || x.Code.Contains(keyword);
            }
            else
            {
                if (request.Status)
                    Criteria = x => x.Status == true;
                else
                    Criteria = x => true;
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