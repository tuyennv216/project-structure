using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class ForgotPasswordRequest
		{
			public required string Email { get; set; }
			public required string Device { get; set; }
		}

		[HttpPost("/api/auth/forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
		{
			var result = await _identityForgotPassword.ForgotPasswordAsync(request.Email, request.Device);
			return Ok(result);
		}
	}
}
