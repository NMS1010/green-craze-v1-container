using System.Security.Claims;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IJwtService
    {
        Task<string> CreateJWT(string userId);

        ClaimsPrincipal ValidateExpiredJWT(string token);

        string CreateRefreshToken();
    }
}