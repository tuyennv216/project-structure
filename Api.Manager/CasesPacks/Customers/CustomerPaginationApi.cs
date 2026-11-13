using Api.Manager.Options;
using Core.Metadata.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pack.Information.Entities;
using Pack.Information.Metadata;

namespace Api.Manager.CasesPacks.Customers
{
	public partial class CustomerController : ControllerBase
	{
		public class CustomerPaginationApi
		{
			public required int Index { get; set; }
			public required int Size { get; set; }
			public Guid Id { get; set; } = Guid.Empty;
			public string Name { get; set; } = string.Empty;
			public string PhoneNumber { get; set; } = string.Empty;
			public int Status { get; set; } = 0;
		}

		public class CustomerPaginationValidator : AbstractValidator<CustomerPaginationApi>
		{
			public CustomerPaginationValidator()
			{
				RuleFor(x => x.Index).GreaterThan(0).WithMessage("Index không hợp lệ");
				RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size không hợp lệ");
				RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage("UserId không hợp lệ");
				RuleFor(x => x.Status).Must(status => CustomerStatus.All.Contains(status)).WithMessage("Status không hợp lệ");
			}
		}

		[HttpPost("/api-manager/customer/pagination")]
		[Authorize]
		public async Task<IActionResult> Pagination([FromBody] CustomerPaginationApi request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new CustomerPaginationValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var filter = new Customer
			{
				Name = request.Name,
				PhoneNumber = request.PhoneNumber,
				Status = request.Status,
			};

			var pagination = new PaginationFilter
			{
				Index = request.Index,
				Size = request.Size,
			};

			var result = await _customerReadPagination.ReadPaginationAsync(filter, pagination);
			return Ok(result);
		}
	}
}
