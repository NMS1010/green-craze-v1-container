using FluentValidation;
using green_craze_be_v1.Application.Model.User;

namespace green_craze_be_v1.Application.Validators.User
{
    public class CreateStaffRequestValidator : AbstractValidator<CreateStaffRequest>
    {
        public CreateStaffRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.Type).NotEmpty().NotNull();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
            RuleFor(x => x.Dob).NotEmpty().NotNull();
            RuleFor(x => x.FirstName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}