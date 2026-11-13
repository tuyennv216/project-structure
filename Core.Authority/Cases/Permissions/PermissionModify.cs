using Core.Authority.Contexts;
using Core.Authority.Entities;
using Core.Authority.Metadata;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionModify
	{
		private readonly AuthorityDb _authorityDb;
		private readonly PermissionRequest _permissionRequest;
		private readonly IActiveUser _activeUser;

		public PermissionModify(AuthorityDb authorityDb, PermissionRequest permissionRequest, IActiveUser activeUser)
		{
			this._authorityDb = authorityDb;
			this._permissionRequest = permissionRequest;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> ModifyAsync(Permission permission)
		{
			var isAdmin = _permissionRequest.Request(_activeUser.Permissions, RoleNames.Admin);
			var canModify = _permissionRequest.Request(_activeUser.Permissions, AccessNames.Modify, ComponentNames.Authority);
			if (!isAdmin || !canModify) return new CodeResult { Code = 0, Error = "Unauthorized" };

			var permissionEntity = await _authorityDb.Permissions.FindAsync(permission.UserId, permission.TargetName);

			if (permissionEntity == null) return new CodeResult { Code = 0, Error = "Permission not found" };

			if (Array.IndexOf(PermissionStatus.All, permission.Status) == -1) return new CodeResult { Code = 0, Error = "Status not valid" };

			permissionEntity.DateStart = permission.DateStart;
			permissionEntity.DateEnd = permission.DateEnd;
			permissionEntity.Status = permission.Status;

			_authorityDb.Update(permissionEntity);
			await _authorityDb.SaveChangesAsync();

			return new CodeResult { Code = 1 };
		}
	}
}
