using Api.Manager.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesPacks.Customers
{
	public partial class CustomerController : ControllerBase
	{
		public class CustomerApproveRequest
		{
			public required Guid Id { get; set; }
		}

		public class CustomerApproveValidator : AbstractValidator<CustomerApproveRequest>
		{
			public CustomerApproveValidator()
			{
				RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage("Id không hợp lệ");
			}
		}

		[HttpPost("/api-manager/customer/approve")]
		[Authorize]
		public async Task<IActionResult> Approve([FromBody] CustomerApproveRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var validator = new CustomerApproveValidator();
			var validateResult = await validator.ValidateAsync(request);
			if (!validateResult.IsValid) return BadRequest(ValidateErrors.Get(validateResult));

			var result = await _customerApprove.ApproveAsync(request.Id);
			return Ok(result);
		}
	}
}
