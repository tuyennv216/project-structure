using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pack.Information.Entities;

namespace Api.Public.Cases.Customers
{
	public partial class CustomerController : ControllerBase
	{
		public class CustomerModifyRequest
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public string PhoneNumber { get; set; }
		}

		[HttpPost("/api/customer/modify")]
		[Authorize]
		public async Task<IActionResult> Modify([FromBody] CustomerModifyRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}
			
			var customer = new Customer
			{
				Id = request.Id,
				Name = request.Name,
				PhoneNumber = request.PhoneNumber,
			};

			var result = await _customerModify.ModifyAsync(customer);
			return Ok(result);
		}
	}
}
