using Core.Identity.Cases.Identities;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesPacks.Identities
{
	[ApiController]
	public partial class IdentityController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly IdentityLogin _identityLogin;
		private readonly IdentityLogout _identityLogout;

		public IdentityController(
			IActiveUser activeUser,
			IdentityLogin accountLogin,
			IdentityLogout accountLogout)
		{
			_activeUser = activeUser;
			_identityLogin = accountLogin;
			_identityLogout = accountLogout;
		}
	}
}
