using FluentValidation;
using green_craze_be_v1.Application.Model.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Cart
{
    public class AddVariantItemToCartRequestValidator : AbstractValidator<CreateCartItemRequest>
    {
        public AddVariantItemToCartRequestValidator()
        {
            RuleFor(x => x.VariantId).NotEmpty().NotNull();
        }
    }
}