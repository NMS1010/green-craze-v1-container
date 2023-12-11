using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Common.Options;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace green_craze_be_v1.Application.Services
{
	public class JwtService : IJwtService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IConfiguration _configuration;

		public JwtService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		public async Task<string> CreateJWT(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			var roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>()
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.GivenName, user.FirstName + user.LastName),
				new(ClaimTypes.Name, user.UserName)
			};
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			var jwtOptions = _configuration.GetOptions<JWTConfigOptions>("Tokens");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(jwtOptions.Issuer,
				jwtOptions.Issuer,
				claims,
				expires: DateTime.Now.AddMinutes(jwtOptions.Expired),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string CreateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}

		public ClaimsPrincipal ValidateExpiredJWT(string token)
		{
			IdentityModelEventSource.ShowPII = true;

			var jwtOptions = _configuration.GetOptions<JWTConfigOptions>("Tokens");
			TokenValidationParameters validationParameters = new()
			{
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				ValidAudience = jwtOptions.Issuer,
				ValidIssuer = jwtOptions.Issuer,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
			};

			ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken validatedToken);
			if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				return null;

			return principal;
		}
	}
}