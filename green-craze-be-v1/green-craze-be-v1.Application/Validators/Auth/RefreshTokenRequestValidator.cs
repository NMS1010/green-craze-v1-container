using FluentValidation;
using green_craze_be_v1.Application.Model.Auth;

namespace green_craze_be_v1.Application.Validators.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty().NotNull();
            RuleFor(x => x.RefreshToken).NotEmpty().NotNull();
        }
    }
}