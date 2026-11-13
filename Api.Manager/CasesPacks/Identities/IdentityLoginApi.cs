using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesPacks.Identities
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

		public class LoginValidator : AbstractValidator<LoginRequest>
		{
			public LoginValidator()
			{
				RuleFor(x => x.Email).NotEmpty().WithMessage("Email không hợp lệ");
				RuleFor(x => x.Password).NotEmpty().WithMessage("Password không hợp lệ");
				RuleFor(x => x.Device).NotEmpty().WithMessage("Device không hợp lệ");
				//RuleFor(x => x.Passcode).NotEmpty().WithMessage("Passcode không hợp lệ");
			}
		}

		[HttpPost("/api-manager/auth/login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var validator = new LoginValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var result = await _identityLogin.LoginAsync(request.Email, request.Password, request.Device, request.Passcode);
			return Ok(result);
		}
	}
}
