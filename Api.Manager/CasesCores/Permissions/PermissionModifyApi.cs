using Api.Manager.Options;
using Core.Authority.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionController : ControllerBase
	{
		public class PermissionModifyRequest
		{
			public Guid UserId { get; set; }
			public required string TargetName { get; set; }
			public DateTime DateStart { get; set; }
			public DateTime DateEnd { get; set; }
			public int Status { get; set; }
		}

		public class PermissionModifyValidator : AbstractValidator<PermissionModifyRequest>
		{
			public PermissionModifyValidator()
			{
				RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu tên TargetName");
				RuleFor(x => x.DateStart).NotEqual(default(DateTime)).WithMessage("StartDate không hợp lệ");
				RuleFor(x => x.DateEnd).NotEqual(default(DateTime)).WithMessage("EndDate không hợp lệ");
				RuleFor(x => x.Status).NotEmpty().WithMessage("Yêu cầu Status");
			}
		}

		[HttpPost("/api-manager/permission/modify")]
		[Authorize]
		public async Task<IActionResult> Modify([FromBody] PermissionModifyRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionModifyValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var permission = new Permission
			{
				UserId = request.UserId,
				TargetName = request.TargetName,
				DateStart = request.DateStart,
				DateEnd = request.DateEnd,
				Status = request.Status,
			};
			
			var result = await _permissionModify.ModifyAsync(permission);
			return Ok(result);
		}
	}
}
