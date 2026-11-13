using Core.History.Cases;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;
using Pack.Information.Cases.Customers;

namespace Api.Manager.CasesPacks.Customers
{
	[ApiController]
	public partial class CustomerController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly CustomerApprove _customerApprove;
		private readonly CustomerReadPagination _customerReadPagination;

		public CustomerController(
			IActiveUser activeUser,
			CustomerApprove customerApprove,
			CustomerReadPagination customerReadPagination)
		{
			this._activeUser = activeUser;
			this._customerApprove = customerApprove;
			this._customerReadPagination = customerReadPagination;
		}
	}
}
