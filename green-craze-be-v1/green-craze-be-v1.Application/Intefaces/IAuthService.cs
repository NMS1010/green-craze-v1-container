using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Auth;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IAuthService
    {
        Task<AuthDto> Authenticate(LoginRequest request);

        Task<AuthDto> AuthenticateWithGoogle(GoogleAuthRequest request);

        Task<string> Register(RegisterRequest request, bool isGoogleAuthen = false);

        Task<AuthDto> RefreshToken(RefreshTokenRequest request);

        Task<bool> VerifyOTP(VerifyOTPRequest request);

        Task<bool> ResendOTP(ResendOTPRequest request);

        Task<bool> ForgotPassword(ForgotPasswordRequest request);

        Task<bool> ResetPassword(ResetPasswordRequest request);

        Task RevokeRefreshToken(string userId);

        Task RevokeAllRefreshToken();
    }
}