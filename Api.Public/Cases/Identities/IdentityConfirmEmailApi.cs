using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class ConfirmEmailRequest
		{
			public required string Email { get; set; }
			public required string LoginEmailToken { get; set; }
		}

		[HttpGet("/api/auth/confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
		{
			var result = await _identityConfirmEmail.ConfirmEmailAsync(request.Email, request.LoginEmailToken);
			return Ok(result);
		}
	}
}
