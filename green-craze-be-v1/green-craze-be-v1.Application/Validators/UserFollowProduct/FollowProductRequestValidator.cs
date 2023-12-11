using FluentValidation;
using green_craze_be_v1.Application.Model.UserFollowProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.UserFollowProduct
{
    public class FollowProductRequestValidator : AbstractValidator<FollowProductRequest>
    {
        public FollowProductRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
        }
    }
}