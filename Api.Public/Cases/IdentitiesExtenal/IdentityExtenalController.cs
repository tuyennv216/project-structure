using Core.Identity.Cases.IdentitiesExternal;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.IdentitiesExtenal
{
	[ApiController]
	public partial class IdentityExtenalController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ExternalLoginGoogle _externalLoginGoogle;

		public IdentityExtenalController(IConfiguration configuration, ExternalLoginGoogle externalLoginGoogle)
		{
			_configuration = configuration;
			_externalLoginGoogle = externalLoginGoogle;
		}
	}
}
