using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Cases.Customers
{
	public partial class CustomerController : ControllerBase
	{
		public class CustomerReadRequest
		{
			public Guid Id { get; set; }
		}

		[HttpPost("/api/customer/read")]
		[Authorize]
		public async Task<IActionResult> Read([FromBody] CustomerReadRequest request)
		{
			if (_activeUser.Invalid)
			{
				return Unauthorized(new { Code = 401, Error = "Unauthorized" });
			}

			var result = await _customerRead.ReadAsync(request.Id);
			return Ok(result);
		}
	}
}
