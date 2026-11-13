using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class ResetPasswordRequest
		{
			public required string Email { get; set; }
			public required string EmailToken { get; set; }
			public required string NewPassword { get; set; }
		}

		[HttpGet("/api/auth/reset-password")]
		public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest request)
		{
			var result = await _identityResetPassword.ResetPasswordAsync(request.Email, request.EmailToken, request.NewPassword);

			return Ok(result);
		}
	}
}
