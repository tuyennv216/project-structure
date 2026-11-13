using Api.Manager.Options;
using Core.History.Entities;
using Core.Metadata.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	public partial class HistoryAdminController : ControllerBase
	{
		public class HistoryReadPaginationAdminRequest
		{
			public required int Index { get; set; }
			public required int Size { get; set; }
			public Guid UserId { get; set; }
			public required string TargetName { get; set; }
		}

		public class HistoryReadPaginationAdminValidator : AbstractValidator<HistoryReadPaginationAdminRequest>
		{
			public HistoryReadPaginationAdminValidator()
			{
				RuleFor(x => x.Index).GreaterThan(0).WithMessage("Index không hợp lệ");
				RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size không hợp lệ");
				RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
			}
		}

		[HttpPost("/api-manager/history/read-pagination")]
		[Authorize]
		public async Task<IActionResult> ReadPagination([FromBody] HistoryReadPaginationAdminRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new HistoryReadPaginationAdminValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var pagination = new PaginationFilter
			{
				Index = request.Index,
				Size = request.Size,
			};

			var snapshot = new Snapshot
			{
				UserId = request.UserId,
				TargetName = request.TargetName,
			};

			var result = await _historyReadPaginationAdmin.ReadPaginationAdminAsync(snapshot, pagination);
			return Ok(result);
		}
	}
}
