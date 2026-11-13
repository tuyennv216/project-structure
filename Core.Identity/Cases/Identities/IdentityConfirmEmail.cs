using Core.Identity.Contexts;
using Core.Identity.Metadata.Status;
using Core.Metadata.Results;

namespace Core.Identity.Cases.Identities
{
	public class IdentityConfirmEmail
	{
		private readonly IdentityDb _identityDb;

		public IdentityConfirmEmail(IdentityDb identityDb)
		{
			_identityDb = identityDb;
		}

		public async Task<CodeResult> ConfirmEmailAsync(string email, string loginEmailToken)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == email && l.EmailToken == loginEmailToken);
			if (loginEntity == null)
			{
				return new CodeResult { Code = RegisterStatus.InvalidEmailToken, Error = "Invalid token" };
			}

			if (loginEntity.Status != LoginStatus.NeedConfirmEmail)
			{
				return new CodeResult { Code = RegisterStatus.EmailConfirmed, Error = "Email already confirmed" };
			}
			else
			{
				loginEntity.Status = LoginStatus.Ok;
				loginEntity.EmailToken = string.Empty;
				await _identityDb.SaveChangesAsync();
			}

			return new CodeResult { Code = RegisterStatus.Ok, Data = "Please log in again" };
		}
	}
}
