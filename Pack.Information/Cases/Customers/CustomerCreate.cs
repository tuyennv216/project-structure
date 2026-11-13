using Core.Authority.Cases.Permissions;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Pack.Information.Contexts;
using Pack.Information.Entities;
using Pack.Information.Metadata;

namespace Pack.Information.Cases.Customers
{
	public class CustomerCreate
	{
		private readonly InfoDb _infoDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public CustomerCreate(InfoDb infoDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_infoDb = infoDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<bool> CreateAsync(Customer customer)
		{
			var isOwner = customer.Id == _activeUser.UserId;
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canCreate = _permission.Request(_activeUser.Permissions, AccessNames.Create, EntityNames.Customer);

			if (!isOwner && !isAdmin && !canCreate) return false;

			customer.Status = CustomerStatus.New;

			await _infoDb.Customers.AddAsync(customer);
			await _infoDb.SaveChangesAsync();
			return true;
		}
	}
}
