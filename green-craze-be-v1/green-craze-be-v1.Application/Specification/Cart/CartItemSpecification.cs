using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Cart
{
    public class CartItemSpecification : BaseSpecification<CartItem>
    {
        public CartItemSpecification(long cartId, long variantId)
            : base(x => x.Variant.Id == variantId && x.Cart.Id == cartId)
        {
            AddInclude(x => x.Cart);
            AddInclude(x => x.Variant);
        }

        public CartItemSpecification(long cartItemId, string userId)
            : base(x => x.Id == cartItemId && x.Cart.UserId == userId)
        {
            AddInclude(x => x.Cart);
            AddInclude(x => x.Variant);
            AddInclude(x => x.Variant.Product);
        }

        public CartItemSpecification(GetCartPagingRequest request, bool isPaging = false) : base(x => x.Cart.UserId == request.UserId)
        {
            AddInclude(x => x.Cart);
            AddInclude(x => x.Variant);
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}