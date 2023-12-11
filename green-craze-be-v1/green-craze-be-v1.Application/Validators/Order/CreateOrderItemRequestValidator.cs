using FluentValidation;
using green_craze_be_v1.Application.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Order
{
    public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemRequestValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.VariantId).NotEmpty().NotNull();
        }
    }
}