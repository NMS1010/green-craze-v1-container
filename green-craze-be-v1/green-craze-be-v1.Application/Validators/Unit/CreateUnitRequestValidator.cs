using FluentValidation;
using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Validators.Unit
{
    public class CreateUnitRequestValidator : AbstractValidator<CreateUnitRequest>
    {
        public CreateUnitRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}