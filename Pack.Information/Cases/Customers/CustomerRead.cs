using Core.Authority.Cases.Permissions;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Pack.Information.Contexts;
using Pack.Information.Entities;

namespace Pack.Information.Cases.Customers
{
	public class CustomerRead
	{
		private readonly InfoDb _infoDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public CustomerRead(InfoDb infoDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_infoDb = infoDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<Customer?> ReadAsync(Guid customerId)
		{
			var isOwner = customerId == _activeUser.UserId;
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canRead = _permission.Request(_activeUser.Permissions, AccessNames.Read, EntityNames.Customer);

			if (!isOwner && !isAdmin && !canRead) return null;

			var customer = await _infoDb.Customers.FindAsync(customerId);
			return customer;
		}
	}
}
