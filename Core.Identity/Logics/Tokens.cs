using Core.Metadata.Configurations;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Identity.Logics
{
	internal static class Tokens
	{
		public static string GenerateToken(JwtSettings jwtSettings, Dictionary<string, object> claims, DateTime expiredAt)
		{
			var tokenHandler = new JsonWebTokenHandler();

			var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
			var securityKey = new SymmetricSecurityKey(key);
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Claims = claims,
				Expires = expiredAt,
				Issuer = jwtSettings.Issuer,
				Audience = jwtSettings.Audience,
				SigningCredentials = credentials
			};

			return tokenHandler.CreateToken(tokenDescriptor);
		}
	}
}
