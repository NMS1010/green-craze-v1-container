using FluentValidation;
using green_craze_be_v1.Application.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Order
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.PaymentMethodId).NotEmpty().NotNull();
            RuleFor(x => x.DeliveryId).NotEmpty().NotNull();
            RuleFor(x => x.Items).NotNull();
        }
    }
}