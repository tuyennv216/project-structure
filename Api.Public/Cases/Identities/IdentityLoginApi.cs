using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class LoginRequest
		{
			public required string Email { get; set; }
			public required string Password { get; set; }
			public required string Device { get; set; }
			public required string Passcode { get; set; }
		}

		[HttpPost("/api/auth/login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var result = await _identityLogin.LoginAsync(request.Email, request.Password, request.Device, request.Passcode);
			return Ok(result);
		}
	}
}
