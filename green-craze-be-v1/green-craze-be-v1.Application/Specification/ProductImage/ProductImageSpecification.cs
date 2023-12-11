using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.ProductImage
{
    public class ProductImageSpecification : BaseSpecification<Domain.Entities.ProductImage>
    {
        public ProductImageSpecification(long productId) : base(x => x.Product.Id == productId)
        {
            AddInclude(x => x.Product);
        }

        public ProductImageSpecification(long productId, bool isDefault = true) 
            : base(x => x.Product.Id == productId && x.IsDefault == true) { }
    }
}
