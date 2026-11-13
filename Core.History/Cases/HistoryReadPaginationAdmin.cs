using Core.Authority.Cases.Permissions;
using Core.History.Contexts;
using Core.History.Entities;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.History.Cases
{
	public class HistoryReadPaginationAdmin
	{
		private readonly HistoryDb _historyDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public HistoryReadPaginationAdmin(HistoryDb historyDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_historyDb = historyDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<CodeResult> ReadPaginationAdminAsync(Snapshot filter, PaginationFilter pagination)
		{
			// Chỉ dành cho Admin hoặc có quyền đọc
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canRead = _permission.Request(_activeUser.Permissions, AccessNames.Read, EntityNames.History);

			// Kích thước lấy tối đa 100 items 1 lần
			if (pagination.Size > 100) pagination.Size = 100;

			var items = await _historyDb.Snapshots.AsNoTracking()
				.Where(i => i.UserId == filter.UserId)
				.Where(i => string.IsNullOrEmpty(filter.TargetName) || i.TargetName.Contains(filter.TargetName))

				.Skip(pagination.Index)
				.Take(pagination.Size)
				.ToListAsync();

			var result = new PaginationResult
			{
				Index = pagination.Index,
				Size = pagination.Size,
				Data = items,
			};

			return new CodeResult
			{
				Code = 1,
				Data = result,
			};
		}
	}
}
