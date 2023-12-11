using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Cart
{
    public class CartSpecification : BaseSpecification<Domain.Entities.Cart>
    {
        public CartSpecification(string userId) : base(x => x.UserId == userId)
        {
            AddInclude(x => x.CartItems);
        }
    }
}