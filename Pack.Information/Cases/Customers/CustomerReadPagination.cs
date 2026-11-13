using Core.Authority.Cases.Permissions;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;
using Microsoft.EntityFrameworkCore;
using Pack.Information.Contexts;
using Pack.Information.Entities;
using Pack.Information.Metadata;

namespace Pack.Information.Cases.Customers
{
	public class CustomerReadPagination
	{
		private readonly InfoDb _infoDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public CustomerReadPagination(InfoDb infoDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_infoDb = infoDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<CodeResult> ReadPaginationAsync(Customer filter, PaginationFilter pagination)
		{
			// Chỉ dành cho Admin hoặc có quyền đọc
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canRead = _permission.Request(_activeUser.Permissions, AccessNames.Read, EntityNames.Customer);

			if (!isAdmin || !canRead)
				return new CodeResult
				{
					Code = 0,
					Error = "No permission",
				};

			// Kích thước lấy tối đa 100 items 1 lần
			if (pagination.Size > 100) pagination.Size = 100;

			var items = await _infoDb.Customers.AsNoTracking()
				.Where(i => filter.Id == Guid.Empty || i.Id == filter.Id)
				.Where(i => string.IsNullOrEmpty(filter.Name) || i.Name.Contains(filter.Name))
				.Where(i => string.IsNullOrEmpty(filter.PhoneNumber) || i.PhoneNumber.Contains(filter.PhoneNumber))
				.Where(i => filter.Status == CustomerStatus.None || i.Status == filter.Status)

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
