using Core.Identity.Contexts;
using Core.Identity.Metadata.Status;
using Core.Metadata.Results;
using Core.Metadata.Securities;
using System.Security.Cryptography;

namespace Core.Identity.Cases.Identities
{
	public class IdentityForgotPassword
	{
		private readonly IdentityDb _identityDb;

		public IdentityForgotPassword(IdentityDb identityDb)
		{
			_identityDb = identityDb;
		}

		public async Task<CodeResult> ForgotPasswordAsync(string email, string device)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email);
			if (loginEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email" };
			}

			if (loginEntity.Status != LoginStatus.Ok)
			{
				return new CodeResult { Code = LoginStatus.None, Error = "Please try again" };
			}

			var emailToken = RandomNumberGenerator.GetString(StringSets.SecureChars, 32);
			loginEntity.Status = LoginStatus.ForgotPassword;
			loginEntity.EmailToken = emailToken;

			// Send email token

			await _identityDb.SaveChangesAsync();

			return new CodeResult { Code = LoginStatus.Ok, Error = "Password reset email sent" };
		}
	}
}
