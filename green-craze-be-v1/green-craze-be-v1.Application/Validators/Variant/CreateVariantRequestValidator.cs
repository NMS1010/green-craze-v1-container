using FluentValidation;
using green_craze_be_v1.Application.Model.Variant;

namespace green_craze_be_v1.Application.Validators.Variant
{
    public class CreateVariantRequestValidator : AbstractValidator<CreateVariantRequest>
    {
        public CreateVariantRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Sku).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.ItemPrice).NotEmpty().NotNull();
            RuleFor(x => x.TotalPrice).NotEmpty().NotNull();
        }
    }
}