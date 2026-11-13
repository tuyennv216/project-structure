using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class LogoutRequest
		{
			public required string Device { get; set; }
		}

		[HttpPost("/api/auth/logout")]
		[Authorize]
		public IActionResult Logout([FromBody] LogoutRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var result = _identityLogout.LogoutAsync(request.Device);

			return Ok(result);
		}
	}
}
