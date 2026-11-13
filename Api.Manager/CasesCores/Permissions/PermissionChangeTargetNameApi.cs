using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionController : ControllerBase
	{
		public class ChangeTargetNameRequest
		{
			public required string OldName { get; set; }
			public required string NewName { get; set; }
		}

		public class PermissionChangeTargetNameValidator : AbstractValidator<ChangeTargetNameRequest>
		{
			public PermissionChangeTargetNameValidator()
			{
				RuleFor(x => x.OldName).NotEmpty().WithMessage("Yêu cầu chuỗi OldName");
				RuleFor(x => x.NewName).NotEmpty().WithMessage("Yêu cầu chuỗi NewName")
					.NotEqual(x => x.OldName).WithMessage("NewName phải khác OldName");
			}
		}

		[HttpPost("/api-manager/authority/change-target-name")]
		[Authorize]
		public async Task<IActionResult> ChangeTargetName([FromBody] ChangeTargetNameRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionChangeTargetNameValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var result = await _permissionChangeTargetName.ChangeTargetNameAsync(request.OldName, request.NewName);
			return Ok(result);
		}
	}
}
