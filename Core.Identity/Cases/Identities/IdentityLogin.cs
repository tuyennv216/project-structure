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
	public class IdentityLogin
	{
		private readonly IdentityDb _identityDb;
		private readonly JwtSettings _jwtSettings;
		private readonly PermissionGet _permissionGet;

		public IdentityLogin(IdentityDb identityDb, IOptions<JwtSettings> jwtOptions, PermissionGet permissionGet)
		{
			_identityDb = identityDb;
			_jwtSettings = jwtOptions.Value;
			_permissionGet = permissionGet;
		}

		public async Task<CodeResult> LoginAsync(string email, string password, string device, string passcode)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email);
			if (loginEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email or password" };
			}

			if (loginEntity.Status == LoginStatus.Locked)
			{
				return new CodeResult { Code = LoginStatus.Locked, Error = "Account is locked" };
			}

			bool isPasswordValid = Passwords.VerifyPassword(password, loginEntity.Salt, loginEntity.PasswordHash);
			if (!isPasswordValid)
			{
				loginEntity.FailedCount++;
				if (loginEntity.FailedCount > 10)
					loginEntity.Status = LoginStatus.Locked;

				await _identityDb.SaveChangesAsync();
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email or password" };
			}

			if (loginEntity.Device != device)
			{
				loginEntity.RefreshToken = string.Empty;
			}

			if (loginEntity.Status == LoginStatus.NeedConfirmEmail)
			{
				return new CodeResult { Code = LoginStatus.NeedConfirmEmail, Error = "Please confirm your email" };
			}

			if (loginEntity.EnablePasscode && loginEntity.Passcode != passcode)
			{
				return new CodeResult { Code = LoginStatus.NeedPasscode, Error = "Invalid passcode" };
			}

			loginEntity.FailedCount = 0;

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

			var refreshTokenClaims = new Dictionary<string, object>();
			var refreshTokenExpiredAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays);
			string refreshToken = Tokens.GenerateToken(_jwtSettings, refreshTokenClaims, refreshTokenExpiredAt);

			loginEntity.Token = token;
			loginEntity.RefreshToken = refreshToken;
			loginEntity.Device = device;
			loginEntity.Status = LoginStatus.Ok;

			return new CodeResult {
				Code = LoginStatus.Ok,
				Data = new { UserId = userEntity.UserId, Token = token, RefreshToken = refreshToken }
			};
		}
	}
}
