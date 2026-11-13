using Core.Authority.Cases.Permissions;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Pack.Information.Contexts;
using Pack.Information.Metadata;

namespace Pack.Information.Cases.Customers
{
	public class CustomerApprove
	{
		private readonly InfoDb _infoDb;
		private readonly PermissionRequest _permission;
		private readonly IActiveUser _activeUser;

		public CustomerApprove(InfoDb infoDb, PermissionRequest permission, IActiveUser activeUser)
		{
			_infoDb = infoDb;
			_permission = permission;
			_activeUser = activeUser;
		}

		public async Task<bool> ApproveAsync(Guid customerId)
		{
			var isAdmin = _permission.Request(_activeUser.Permissions, RoleNames.Admin);
			var canApprove = _permission.Request(_activeUser.Permissions, AccessNames.Approve, EntityNames.Incident);

			if (!isAdmin && !canApprove) return false;

			var existingIncident = await _infoDb.Customers.FindAsync(customerId);
			if (existingIncident == null) return false;

			existingIncident.Status = CustomerStatus.Ok;

			_infoDb.Customers.Update(existingIncident);
			await _infoDb.SaveChangesAsync();
			return true;
		}
	}
}
