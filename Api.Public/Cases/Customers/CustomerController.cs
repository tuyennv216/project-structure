using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;
using Pack.Information.Cases.Customers;

namespace Api.Public.Cases.Customers
{
	[ApiController]
	public partial class CustomerController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly CustomerCreate _customerCreate;
		private readonly CustomerModify _customerModify;
		private readonly CustomerRead _customerRead;

		public CustomerController(
			IActiveUser activeUser,
			CustomerCreate customerCreate,
			CustomerModify customerModify,
			CustomerRead customerRead)
		{
			this._activeUser = activeUser;
			this._customerCreate = customerCreate;
			this._customerModify = customerModify;
			this._customerRead = customerRead;
		}
	}
}
