using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class RegisterRequest
		{
			public required string Email { get; set; }
			public required string Password { get; set; }
		}

		[HttpPost("/api/auth/register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var result = await _identityRegister.RegisterAsync(request.Email, request.Password);

			return Ok(result);
		}
	}
}
