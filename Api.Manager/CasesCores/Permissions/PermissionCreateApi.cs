using Api.Manager.Options;
using Core.Authority.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionController : ControllerBase
	{
		public class PermissionCreateRequest
		{
			public Guid UserId { get; set; }
			public required string TargetName { get; set; }
			public DateTime DateStart { get; set; }
			public DateTime DateEnd { get; set; }
		}

		public class PermissionCreateValidator : AbstractValidator<PermissionCreateRequest>
		{
			public PermissionCreateValidator()
			{
				RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
				RuleFor(x => x.DateStart).NotEqual(default(DateTime)).WithMessage("StartDate không hợp lệ");
				RuleFor(x => x.DateEnd).NotEqual(default(DateTime)).WithMessage("DateEnd không hợp lệ");
			}
		}

		[HttpPost("/api-manager/permission/create")]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] PermissionCreateRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionCreateValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var permission = new Permission
			{
				UserId = request.UserId,
				TargetName = request.TargetName,
				DateStart = request.DateStart,
				DateEnd = request.DateEnd,
			};

			var result = await _permissionCreate.CreateAsync(permission);
			return Ok(result);
		}
	}
}
