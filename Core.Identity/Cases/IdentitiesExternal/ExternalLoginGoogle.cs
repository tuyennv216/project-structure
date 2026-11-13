using Core.Authority.Cases.Permissions;
using Core.Identity.Contexts;
using Core.Identity.Entities;
using Core.Identity.Logics;
using Core.Identity.Metadata.Status;
using Core.Metadata.Configurations;
using Core.Metadata.Names;
using Core.Metadata.Results;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Identity.Cases.IdentitiesExternal
{
	public class ExternalLoginGoogle
	{
		private readonly IdentityDb _identityDb;
		private readonly GoogleSettings _googleOptions;
		private readonly PermissionGet _permissionGet;
		private readonly JwtSettings _jwtSettings;

		public ExternalLoginGoogle(IdentityDb identityDb, IOptions<GoogleSettings> googleOptions, IOptions<JwtSettings> jwtOptions, PermissionGet permissionGet)
		{
			_identityDb = identityDb;
			_googleOptions = googleOptions.Value;
			_jwtSettings = jwtOptions.Value;
			_permissionGet = permissionGet;
		}

		public async Task<CodeResult> LoginByIdToken(string idToken)
		{
			try
			{
				var googleClientId = _googleOptions.ClientId;

				// 1. Xác thực ID Token gửi từ client do Google cấp phát
				var settings = new GoogleJsonWebSignature.ValidationSettings()
				{
					Audience = new List<string> { googleClientId }
				};

				GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

				// 2. Token hợp lệ! Lấy thông tin người dùng từ payload của Google
				var email = payload.Email;
				var fullname = payload.Name;
				var googleId = payload.Subject;
				var avatarUrl = payload.Picture;

				// 3. Kiểm tra trong Database xem user này đã tồn tại chưa:
				var userEntity = await _identityDb.Users.FirstOrDefaultAsync(u => u.Email == email);
				if (userEntity == null)
				{
					// Nếu chưa có -> Tạo mới User
					userEntity = new User
					{
						UserId = Guid.NewGuid(),
						Email = email,
						Fullname = fullname,
						GoogleId = googleId,
						AvatarUrl = avatarUrl,
						Status = UserStatus.New,
						DateStart = DateTime.UtcNow,
					};
					_identityDb.Users.Add(userEntity);

					// Tạo mới Login record tương ứng (không có password/shalt)
					var loginRecord = new Login
					{
						Email = email,
						PasswordHash = null,
						Salt = null,
						Status = UserStatus.New,
					};
					_identityDb.Logins.Add(loginRecord);

					await _identityDb.SaveChangesAsync();
				}

				if (userEntity.GoogleId == string.Empty)
				{
					userEntity.GoogleId = googleId;
					userEntity.AvatarUrl = avatarUrl;
					_identityDb.Users.Update(userEntity);
				}

				var loginEntity = await _identityDb.Logins.FirstOrDefaultAsync(l => l.Email == email);

				if (loginEntity == null) return new CodeResult { Code = 404, Error = "User login record not found" };

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
				loginEntity.Device = string.Empty;
				loginEntity.Status = LoginStatus.Ok;

				return new CodeResult
				{
					Code = LoginStatus.Ok,
					Data = new { UserId = userEntity.UserId, Token = token, RefreshToken = refreshToken }
				};
			}
			catch (InvalidJwtException)
			{
				return new CodeResult { Code = 400, Error = "Google Token không hợp lệ hoặc đã hết hạn" };
			}
			catch (Exception)
			{
				return new CodeResult { Code = 500, Error = "Lỗi hệ thống" };
			}
		}
	}
}
