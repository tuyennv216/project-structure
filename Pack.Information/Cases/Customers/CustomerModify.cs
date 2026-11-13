using Core.Authority.Cases.Permissions;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Pack.Information.Contexts;
using Pack.Information.Entities;
using Pack.Information.Metadata;

namespace Pack.Information.Cases.Customers
{
	public class CustomerModify
	{
		private readonly InfoDb _infoDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public CustomerModify(InfoDb infoDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_infoDb = infoDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<bool> ModifyAsync(Customer customer)
		{
			var isOwner = customer.Id == _activeUser.UserId;
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canModify = _permission.Request(_activeUser.Permissions, AccessNames.Modify, EntityNames.Customer);

			if (!isOwner && !isAdmin && !canModify) return false;

			var customerEntity = await _infoDb.Customers.FindAsync(customer.Id);

			if (customerEntity == null) return false;

			customerEntity.Name = customer.Name;
			customerEntity.PhoneNumber = customer.PhoneNumber;
			customerEntity.Status = CustomerStatus.Modified;

			_infoDb.Customers.Update(customerEntity);
			await _infoDb.SaveChangesAsync();
			return true;
		}
	}
}
