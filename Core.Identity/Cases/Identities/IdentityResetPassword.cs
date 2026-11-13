using Core.Identity.Contexts;
using Core.Identity.Logics;
using Core.Identity.Metadata.Status;
using Core.Metadata.Results;

namespace Core.Identity.Cases.Identities
{
	public class IdentityResetPassword
	{
		private readonly IdentityDb _identityDb;

		public IdentityResetPassword(IdentityDb identityDb)
		{
			_identityDb = identityDb;
		}

		public async Task<CodeResult> ResetPasswordAsync(string email, string emailToken, string newPassword)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email && l.EmailToken == emailToken);
			if (loginEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email or token" };
			}

			if (loginEntity.Status != LoginStatus.ForgotPassword
				|| loginEntity.Status != LoginStatus.NeedUpdatePassword
				|| loginEntity.Status != LoginStatus.Ok)
			{
				return new CodeResult { Code = LoginStatus.None, Error = "Please try again" };
			}

			var userEntity = _identityDb.Users.FirstOrDefault(u => u.Email == email);
			if (userEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email" };
			}

			var salt = Passwords.GenerateSalt();
			var passwordHash = Passwords.HashPassword(newPassword, salt);
			loginEntity.Salt = salt;
			loginEntity.PasswordHash = passwordHash;
			loginEntity.Token = string.Empty;
			loginEntity.RefreshToken = string.Empty;
			loginEntity.ResetPasswordToken = string.Empty;
			loginEntity.EmailToken = string.Empty;
			loginEntity.Passcode = string.Empty;
			loginEntity.Device = string.Empty;
			loginEntity.Status = LoginStatus.Ok;

			await _identityDb.SaveChangesAsync();

			return new CodeResult { Code = LoginStatus.Ok, Data = "Please log in again" };
		}
	}
}
