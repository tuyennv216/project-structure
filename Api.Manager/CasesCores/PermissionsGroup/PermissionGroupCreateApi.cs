using Api.Manager.Options;
using Core.Authority.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class PermissionGroupController : ControllerBase
	{
		public class PermissionGroupCreateRequest
		{
			public Guid[] GroupUsers { get; set; }
			public required string TargetName { get; set; }
			public DateTime DateStart { get; set; }
			public DateTime DateEnd { get; set; }
		}

		public class PermissionGroupCreateValidator : AbstractValidator<PermissionGroupCreateRequest>
		{
			public PermissionGroupCreateValidator()
			{
				RuleFor(x => x.GroupUsers).NotEmpty().WithMessage("GroupUsers không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
				RuleFor(x => x.DateStart).NotEqual(default(DateTime)).WithMessage("StartDate không hợp lệ");
				RuleFor(x => x.DateEnd).NotEqual(default(DateTime)).WithMessage("DateEnd không hợp lệ");

				RuleForEach(x => x.GroupUsers).NotEqual(Guid.Empty).WithMessage("GroupUsers không hợp lệ");
			}
		}

		[HttpPost("/api-manager/permission/group-create")]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] PermissionGroupCreateRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new PermissionGroupCreateValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var permission = new Permission
			{
				TargetName = request.TargetName,
				DateStart = request.DateStart,
				DateEnd = request.DateEnd,
			};

			var result = await _permissionGroupCreate.CreateAsync(permission, request.GroupUsers);
			return Ok(result);
		}
	}
}
