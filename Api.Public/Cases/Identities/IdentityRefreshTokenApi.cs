using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class RefreshTokenRequest
		{
			public required string RefreshToken { get; set; }
		}

		[HttpPost("/api/auth/refresh-token")]
		[Authorize]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var result = await _identityRefreshToken.RefreshTokenAsync(_activeUser.Email, request.RefreshToken);

			return Ok(result);
		}
	}
}
