using Core.Identity.Contexts;
using Core.Identity.Entities;
using Core.Identity.Logics;
using Core.Identity.Metadata.Status;
using Core.Metadata.Results;
using Core.Metadata.Securities;
using System.Security.Cryptography;

namespace Core.Identity.Cases.Identities
{
	public class IdentityRegister
	{
		private readonly IdentityDb _identityDb;

		public IdentityRegister(IdentityDb identityDb)
		{
			_identityDb = identityDb;
		}

		public async Task<CodeResult> RegisterAsync(string email, string password)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email);
			if (loginEntity != null)
			{
				return new CodeResult { Code = RegisterStatus.EmailAlreadyExists, Error = "Email already exists" };
			}

			var userId = Guid.CreateVersion7();
			var newUser = new User
			{
				UserId = userId,
				Email = email,
				DateStart = DateTime.UtcNow,
				DateEnd = DateTime.UtcNow.AddYears(1),
				Status = UserStatus.New,
			};
			await _identityDb.Users.AddAsync(newUser);

			var salt = Passwords.GenerateSalt();
			var passwordHash = Passwords.HashPassword(password, salt);
			var loginEmailToken = RandomNumberGenerator.GetString(StringSets.SecureChars, 32);
			var newLoginEntity = new Login
			{
				Email = email,
				Salt = salt,
				PasswordHash = passwordHash,
				FailedCount = 0,
				EnablePasscode = false,
				Passcode = string.Empty,
				Token = string.Empty,
				RefreshToken = string.Empty,
				EmailToken = loginEmailToken,
				Device = string.Empty,
				Status = LoginStatus.NeedConfirmEmail,
			};
			await _identityDb.Logins.AddAsync(newLoginEntity);

			await _identityDb.SaveChangesAsync();

			// Send Email
			// loginEmailToken

			return new CodeResult { Code = RegisterStatus.Ok, Data = "Please check your email for confirmation." };
		}
	}
}
