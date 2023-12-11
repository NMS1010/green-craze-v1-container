using FluentValidation;
using green_craze_be_v1.Application.Model.PaymentMethod;

namespace green_craze_be_v1.Application.Validators.PaymentMethod
{
    public class UpdatePaymentMethodRequestValidator : AbstractValidator<UpdatePaymentMethodRequest>
    {
        public UpdatePaymentMethodRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}