using green_craze_be_v1.Application.Intefaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace green_craze_be_v1.Application.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool IsInRole(string role)
		{
			var userRoles = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role);

			return userRoles.FirstOrDefault(x => x.Value == role) != null;
		}
	}
}