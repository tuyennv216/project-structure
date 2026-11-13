using Core.Authority.Cases.Permissions;
using Core.Identity.Contexts;
using Core.Identity.Logics;
using Core.Identity.Metadata.Status;
using Core.Metadata.Configurations;
using Core.Metadata.Names;
using Core.Metadata.Results;
using Microsoft.Extensions.Options;

namespace Core.Identity.Cases.Identities
{
	public class IdentityRefreshToken
	{
		private readonly IdentityDb _identityDb;
		private readonly JwtSettings _jwtSettings;
		private readonly PermissionGet _permissionGet;

		public IdentityRefreshToken(IdentityDb identityDb, IOptions<JwtSettings> jwtOptions, PermissionGet permissionGet)
		{
			_identityDb = identityDb;
			_jwtSettings = jwtOptions.Value;
			_permissionGet = permissionGet;
		}

		public async Task<CodeResult> RefreshTokenAsync(string email, string refreshToken)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email && l.RefreshToken == refreshToken);
			if (loginEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Token has expired" };
			}

			if (loginEntity.Status == LoginStatus.Locked)
			{
				return new CodeResult { Code = LoginStatus.Locked, Error = "Account is locked" };
			}

			var userEntity = _identityDb.Users.FirstOrDefault(u => u.Email == email);
			if (userEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email or password" };
			}

			var userPermissions = await _permissionGet.GetAsync(userEntity.UserId);

			var tokenClaims = new Dictionary<string, object>
			{
				[ClaimNames.UserId] = userEntity.UserId,
				[ClaimNames.Email] = userEntity.Email,
				[ClaimNames.Permissions] = userPermissions,
			};
			var tokenExpiredAt = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpireMinutes);
			string token = Tokens.GenerateToken(_jwtSettings, tokenClaims, tokenExpiredAt);
			
			return new CodeResult { Code = LoginStatus.Ok, Data = new { Token = token } };
		}
	}
}
