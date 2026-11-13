using Core.Authority.Contexts;
using Core.Authority.Entities;
using Core.Authority.Metadata;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionCreate
	{
		private readonly AuthorityDb _authorityDb;
		private readonly PermissionRequest _permissionRequest;
		private readonly IActiveUser _activeUser;

		public PermissionCreate(AuthorityDb authorityDb, PermissionRequest permissionRequest, IActiveUser activeUser)
		{
			this._authorityDb = authorityDb;
			this._permissionRequest = permissionRequest;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> CreateAsync(Permission permission)
		{
			var isAdmin = _permissionRequest.Request(_activeUser.Permissions, RoleNames.Admin);
			var canCreate = _permissionRequest.Request(_activeUser.Permissions, AccessNames.Create, ComponentNames.Authority);
			if (!isAdmin || !canCreate) return new CodeResult { Code = 0, Error = "Unauthorized" };

			var permissionEntity = await _authorityDb.Permissions.FindAsync(permission.UserId, permission.TargetName);
			if (permissionEntity != null) return new CodeResult { Code = 0, Error = "Permission already exists" };

			var newPermission = new Permission
			{
				UserId = permission.UserId,
				TargetName = permission.TargetName,
				DateStart = permission.DateStart,
				DateEnd = permission.DateEnd,
				Status = PermissionStatus.Ok,
			};

			await _authorityDb.AddAsync(newPermission);
			await _authorityDb.SaveChangesAsync();

			return new CodeResult { Code = 1 };
		}
	}
}
