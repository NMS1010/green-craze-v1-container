using FluentValidation;
using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Validators.Unit
{
    public class UpdateUnitRequestValidator : AbstractValidator<UpdateUnitRequest>
    {
        public UpdateUnitRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}