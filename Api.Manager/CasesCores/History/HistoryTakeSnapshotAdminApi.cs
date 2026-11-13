using Api.Manager.Options;
using Core.History.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class HistoryAdminController : ControllerBase
	{
		public class HistoryTakeSnapshotAdminRequest
		{
			public Guid UserId { get; set; }
			public required string TargetName { get; set; }
			public string? Data { get; set; }
		}

		public class HistoryTakeSnapshotValidator : AbstractValidator<HistoryTakeSnapshotAdminRequest>
		{
			public HistoryTakeSnapshotValidator()
			{
				RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
				RuleFor(x => x.Data).NotNull().WithMessage("Data không hợp lệ");
			}
		}

		[HttpPost("/api-manager/history/take-snapshot")]
		[Authorize]
		public async Task<IActionResult> TakeSnapshot([FromBody] HistoryTakeSnapshotAdminRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new HistoryTakeSnapshotValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var snapshot = new Snapshot
			{
				UserId = request.UserId,
				TargetName = request.TargetName,
				Data = request.Data,
			};

			var result = await _historyTakeSnapshotAdmin.TakeSnapshotAsync(snapshot);
			return Ok(result);
		}
	}
}
