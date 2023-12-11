using FluentValidation;
using green_craze_be_v1.Application.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Auth
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}