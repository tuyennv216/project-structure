using Core.History.Cases;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	[ApiController]
	public partial class HistoryAdminController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly HistoryTakeSnapshotAdmin _historyTakeSnapshotAdmin;
		private readonly HistoryReadPaginationAdmin _historyReadPaginationAdmin;

		public HistoryAdminController(
			IActiveUser activeUser,
			HistoryTakeSnapshotAdmin historyTakeSnapshotAdmin,
			HistoryReadPaginationAdmin historyReadPaginationAdmin)
		{
			this._activeUser = activeUser;
			this._historyTakeSnapshotAdmin = historyTakeSnapshotAdmin;
			this._historyReadPaginationAdmin = historyReadPaginationAdmin;
		}
	}
}
