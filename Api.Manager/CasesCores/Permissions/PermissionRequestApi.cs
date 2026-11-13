using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionController : ControllerBase
	{
		public class PermissionRequestRequest
		{
			public string PermissionsStr { get; set; }
			public string PermissionName { get; set; }
		}

		public class PermissionRequestValidator : AbstractValidator<PermissionRequestRequest>
		{
			public PermissionRequestValidator()
			{
				RuleFor(x => x.PermissionsStr).NotEmpty().WithMessage("Yêu cầu chuỗi permissionsStr");
				RuleFor(x => x.PermissionName).NotEmpty().WithMessage("Yêu cầu tên permission");
			}
		}

		[HttpPost("/api-manager/permission/request")]
		[Authorize]
		public async Task<IActionResult> Requests([FromBody] PermissionRequestRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionRequestValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var permissions = request.PermissionsStr.Split(';');
			var pername = request.PermissionName;

			var result = _permissionRequest.Request(permissions, pername);
			return Ok(result);
		}
	}
}
