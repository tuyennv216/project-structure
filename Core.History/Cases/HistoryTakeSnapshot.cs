using Core.Authority.Cases.Permissions;
using Core.History.Contexts;
using Core.History.Entities;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;

namespace Core.History.Cases
{
	public class HistoryTakeSnapshot
	{
		private readonly HistoryDb _historyDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public HistoryTakeSnapshot(HistoryDb historyDb, PermissionRequest permission, IActiveUser activeUser)
		{
			this._historyDb = historyDb;
			this._permission = permission;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> TakeSnapshotAsync(Snapshot snapshot)
		{
			var newSnapshot = new Snapshot
			{
				UserId = _activeUser.UserId,
				TargetName = snapshot.TargetName,
				Data = snapshot.Data,
				DateCreated = DateTime.UtcNow
			};

			await _historyDb.Snapshots.AddAsync(newSnapshot);
			await _historyDb.SaveChangesAsync();

			return new CodeResult { Code = 1 };
		}
	}
}
