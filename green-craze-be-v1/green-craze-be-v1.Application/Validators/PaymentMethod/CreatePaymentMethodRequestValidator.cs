using FluentValidation;
using green_craze_be_v1.Application.Model.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.PaymentMethod
{
    public class CreatePaymentMethodRequestValidator : AbstractValidator<CreatePaymentMethodRequest>
    {
        public CreatePaymentMethodRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Image).NotEmpty().NotNull();
        }
    }
}