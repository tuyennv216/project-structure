using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionController : ControllerBase
	{
		public class PermissionGetRequest
		{
			public Guid UserId { get; set; }
		}

		public class PermissionGetValidator : AbstractValidator<PermissionGetRequest>
		{
			public PermissionGetValidator()
			{
				RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
			}
		}

		[HttpPost("/api-manager/permission/get")]
		[Authorize]
		public async Task<IActionResult> Get([FromBody] PermissionGetRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionGetValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var result = await _permissionGet.GetAsync(request.UserId);
			return Ok(result);
		}
	}
}
