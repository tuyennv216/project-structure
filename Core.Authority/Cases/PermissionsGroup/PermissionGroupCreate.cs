using Core.Authority.Contexts;
using Core.Authority.Entities;
using Core.Authority.Metadata;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionGroupCreate
	{
		private readonly AuthorityDb _authorityDb;
		private readonly PermissionRequest _permissionRequest;
		private readonly IActiveUser _activeUser;

		public PermissionGroupCreate(AuthorityDb authorityDb, PermissionRequest permissionRequest, IActiveUser activeUser)
		{
			this._authorityDb = authorityDb;
			this._permissionRequest = permissionRequest;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> CreateAsync(Permission permission, Guid[] groupUsersId)
		{
			var isAdmin = _permissionRequest.Request(_activeUser.Permissions, RoleNames.Admin);
			var canCreate = _permissionRequest.Request(_activeUser.Permissions, AccessNames.Create, ComponentNames.Authority);
			if (!isAdmin || !canCreate) return new CodeResult { Code = 0, Error = "Unauthorized" };
			
			var existsPermissions = _authorityDb.Permissions
				.Where(p => groupUsersId.Contains(p.UserId) && p.TargetName == permission.TargetName)
				.Select(p => p.UserId);

			var newUsersId = groupUsersId.Except(existsPermissions)
				.Select(userId => new Permission
				{
					UserId = userId,
					TargetName = permission.TargetName,
					DateStart = permission.DateStart,
					DateEnd = permission.DateEnd,
					Status = PermissionStatus.Ok,
				});

			await _authorityDb.AddRangeAsync(newUsersId);
			await _authorityDb.SaveChangesAsync();

			return new CodeResult { Code = 1 };
		}
	}
}
