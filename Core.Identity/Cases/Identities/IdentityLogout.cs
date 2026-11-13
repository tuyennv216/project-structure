using Core.Identity.Contexts;
using Core.Identity.Metadata.Status;
using Core.Metadata.Options;
using Core.Metadata.Results;

namespace Core.Identity.Cases.Identities
{
	public class IdentityLogout
	{
		private readonly IdentityDb _identityDb;
		private readonly IActiveUser _activeUser;

		public IdentityLogout(IdentityDb identityDb, IActiveUser activeUser)
		{
			_identityDb = identityDb;
			_activeUser = activeUser;
		}

		public async Task<CodeResult> LogoutAsync(string device)
		{
			var loginEntity = _identityDb.Logins.FirstOrDefault(l => l.Email == _activeUser.Email);
			if (loginEntity == null)
			{
				return new CodeResult { Code = LoginStatus.NotFound, Error = "Invalid email" };
			}

			if (loginEntity.Device != device)
			{
				return new CodeResult { Code = LoginStatus.InvalidDevice, Error = "Invalid device" };
			}

			if (loginEntity.Status != LoginStatus.Ok)
			{
				return new CodeResult { Code = LoginStatus.None, Error = "User is not logged in" };
			}

			loginEntity.Token = string.Empty;
			loginEntity.RefreshToken = string.Empty;
			loginEntity.ResetPasswordToken = string.Empty;
			loginEntity.EmailToken = string.Empty;
			loginEntity.Passcode = string.Empty;
			loginEntity.Device = string.Empty;
			loginEntity.Status = LoginStatus.LoggedOut;

			await _identityDb.SaveChangesAsync();
			return new CodeResult { Code = LoginStatus.Ok, Data = "Successfully logged out" };
		}
	}
}
