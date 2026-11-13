using Core.History.Entities;
using Core.Metadata.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.CasesCores.Permissions
{
	public partial class HistoryController : ControllerBase
	{
		public class HistoryReadPaginationRequest
		{
			public required int Index { get; set; }
			public required int Size { get; set; }
			public required string TargetName { get; set; }
		}

		public class HistoryReadPaginationValidator : AbstractValidator<HistoryReadPaginationRequest>
		{
			public HistoryReadPaginationValidator()
			{
				RuleFor(x => x.Index).GreaterThan(0).WithMessage("Index không hợp lệ");
				RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size không hợp lệ");
				RuleFor(x => x.TargetName).NotEmpty().WithMessage("Yêu cầu chuỗi TargetName");
			}
		}

		[HttpPost("/api/history/read-pagination")]
		[Authorize]
		public async Task<IActionResult> ReadPagination([FromBody] HistoryReadPaginationRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new HistoryReadPaginationValidator();
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

			var pagination = new PaginationFilter
			{
				Index = request.Index,
				Size = request.Size,
			};

			var snapshot = new Snapshot
			{
				TargetName = request.TargetName,
			};

			var result = await _historyReadPagination.ReadPaginationAsync(snapshot, pagination);
			return Ok(result);
		}
	}
}
