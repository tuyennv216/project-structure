using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesPacks.Identities
{
	public partial class IdentityController : ControllerBase
	{
		public class LogoutRequest
		{
			public required string Device { get; set; }
		}

		public class LogoutValidator : AbstractValidator<LogoutRequest>
		{
			public LogoutValidator()
			{
				RuleFor(x => x.Device).NotEmpty().WithMessage("Device không hợp lệ");
			}
		}

		[HttpPost("/api-manager/auth/logout")]
		[Authorize]
		public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new LogoutValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var result = _identityLogout.LogoutAsync(request.Device);

			return Ok(result);
		}
	}
}
