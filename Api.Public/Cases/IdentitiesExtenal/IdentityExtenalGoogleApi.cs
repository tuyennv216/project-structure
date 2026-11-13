using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.IdentitiesExtenal
{
	public partial class IdentityExtenalController : ControllerBase
	{
		public class GoogleLoginRequest
		{
			public required string IdToken { get; set; }
		}

		[HttpPost("/api/auth/login-google")]
		public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginRequest request)
		{
			var result = await _externalLoginGoogle.LoginByIdToken(request.IdToken);
			return Ok(result);
		}
	}
}
