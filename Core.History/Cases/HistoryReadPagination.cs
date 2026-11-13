using Core.History.Contexts;
using Core.History.Entities;
using Core.Metadata.Options;
using Core.Metadata.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.History.Cases
{
	public class HistoryReadPagination
	{
		private readonly HistoryDb _historyDb;
		private readonly IActiveUser _activeUser;

		public HistoryReadPagination(HistoryDb historyDb, IActiveUser activeUser)
		{
			_historyDb = historyDb;
			_activeUser = activeUser;
		}

		public async Task<CodeResult> ReadPaginationAsync(Snapshot filter, PaginationFilter pagination)
		{
			// Kích thước lấy tối đa 100 items 1 lần
			if (pagination.Size > 100) pagination.Size = 100;

			var items = await _historyDb.Snapshots.AsNoTracking()
				.Where(i => i.UserId == _activeUser.UserId)
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
