using Core.History.Cases;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.CasesCores.Permissions
{
	[ApiController]
	public partial class HistoryController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly HistoryTakeSnapshot _historyTakeSnapshot;
		private readonly HistoryReadPagination _historyReadPagination;

		public HistoryController(
			IActiveUser activeUser,
			HistoryTakeSnapshot historyTakeSnapshot,
			HistoryReadPagination historyReadPagination)
		{
			this._activeUser = activeUser;
			this._historyTakeSnapshot = historyTakeSnapshot;
			this._historyReadPagination = historyReadPagination;
		}
	}
}
