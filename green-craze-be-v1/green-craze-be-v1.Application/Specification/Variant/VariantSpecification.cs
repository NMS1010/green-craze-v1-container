using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Model.Variant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Variant
{
    public class VariantSpecification : BaseSpecification<Domain.Entities.Variant>
    {
        public VariantSpecification()
        {
            AddInclude(x => x.Product);
        }

        public VariantSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Product);
            AddInclude(x => x.Product.Images);
        }

        public VariantSpecification(long productId, bool ok = true) : base(x => x.Product.Id == productId)
        {
            AddInclude(x => x.Product);
        }

        public VariantSpecification(GetVariantPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name == keyword;
            }

            AddInclude(x => x.Product);

            if (string.IsNullOrEmpty(query.ColumnName))
                query.ColumnName = "Id";
            AddSorting(query.ColumnName, query.IsSortAscending);

            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}