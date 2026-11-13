using Core.History.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.CasesCores.Permissions
{
	public partial class HistoryController : ControllerBase
	{
		public class HistoryTakeSnapshotAdminRequest
		{
			public required string TargetName { get; set; }
			public string? Data { get; set; }
		}

		public class HistoryTakeSnapshotValidator : AbstractValidator<HistoryTakeSnapshotAdminRequest>
		{
			public HistoryTakeSnapshotValidator()
			{
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
				RuleFor(x => x.Data).NotNull().WithMessage("Data không hợp lệ");
			}
		}

		[HttpPost("/api/history/take-snapshot")]
		[Authorize]
		public async Task<IActionResult> TakeSnapshot([FromBody] HistoryTakeSnapshotAdminRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new HistoryTakeSnapshotValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(new
			{
				Code = 400,
				Errors = validateResult.Errors.Select(e => new
				{
					Field = e.PropertyName,
					Message = e.ErrorMessage
				})
			});

			var snapshot = new Snapshot
			{
				TargetName = request.TargetName,
				Data = request.Data,
			};

			var result = await _historyTakeSnapshot.TakeSnapshotAsync(snapshot);
			return Ok(result);
		}
	}
}
