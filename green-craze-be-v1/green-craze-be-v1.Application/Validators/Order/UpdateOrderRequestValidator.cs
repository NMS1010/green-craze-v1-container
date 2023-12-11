using FluentValidation;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Order;

namespace green_craze_be_v1.Application.Validators.Order
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Status).NotEmpty().NotNull();
            RuleFor(x => x.Status)
                .Must(x => ORDER_STATUS.Status.Contains(x))
                .WithMessage("Unexpected order status");
        }
    }
}