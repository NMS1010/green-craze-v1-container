using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Product;

namespace green_craze_be_v1.Application.Specification.Product
{
    public class ProductSearchSpecification : BaseSpecification<Domain.Entities.Product>
    {
        public ProductSearchSpecification(SearchProductPagingRequest query, bool isPaging = false)
        {
            Criteria = x => x.Name.Contains(query.Search) && x.Status != PRODUCT_STATUS.INACTIVE;

            if (string.IsNullOrEmpty(query.ColumnName))
                query.ColumnName = "Name";
            AddSorting(query.ColumnName, query.IsSortAscending);

            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}