using FluentValidation;
using green_craze_be_v1.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().NotNull();
            RuleFor(x => x.OldPassword).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull();
        }
    }
}