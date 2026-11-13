using Core.Identity.Cases.Identities;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	[ApiController]
	public partial class IdentityController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly IdentityLogin _identityLogin;
		private readonly IdentityLogout _identityLogout;
		private readonly IdentityConfirmEmail _identityConfirmEmail;
		private readonly IdentityForgotPassword _identityForgotPassword;
		private readonly IdentityRefreshToken _identityRefreshToken;
		private readonly IdentityRegister _identityRegister;
		private readonly IdentityResetPassword _identityResetPassword;

		public IdentityController(
			IActiveUser activeUser,
			IdentityLogin accountLogin,
			IdentityLogout accountLogout,
			IdentityConfirmEmail accountConfirmEmail,
			IdentityForgotPassword accountForgotPassword,
			IdentityRefreshToken accountRefreshToken,
			IdentityRegister accountRegister,
			IdentityResetPassword accountResetPassword)
		{
			_activeUser = activeUser;
			_identityLogin = accountLogin;
			_identityLogout = accountLogout;
			_identityConfirmEmail = accountConfirmEmail;
			_identityForgotPassword = accountForgotPassword;
			_identityRefreshToken = accountRefreshToken;
			_identityRegister = accountRegister;
			_identityResetPassword = accountResetPassword;
		}
	}
}
