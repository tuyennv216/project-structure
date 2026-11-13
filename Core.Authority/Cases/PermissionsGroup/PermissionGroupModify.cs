using Core.Authority.Contexts;
using Core.Authority.Entities;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionGroupModify
	{
		private readonly AuthorityDb _authorityDb;
		private readonly PermissionRequest _permissionRequest;
		private readonly IActiveUser _activeUser;

		public PermissionGroupModify(AuthorityDb authorityDb, PermissionRequest permissionRequest, IActiveUser activeUser)
		{
			this._authorityDb = authorityDb;
			this._permissionRequest = permissionRequest;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> ModifyAsync(Permission permission, Guid[] groupUsersId)
		{
			var isAdmin = _permissionRequest.Request(_activeUser.Permissions, RoleNames.Admin);
			var canModify = _permissionRequest.Request(_activeUser.Permissions, AccessNames.Modify, ComponentNames.Authority);
			if (!isAdmin || !canModify) return new CodeResult { Code = 0, Error = "Unauthorized" };

			await _authorityDb.Permissions
				.Where(p => groupUsersId.Contains(p.UserId) && p.TargetName == permission.TargetName)
				.ExecuteUpdateAsync(s => s
					.SetProperty(p => p.DateStart, permission.DateStart)
					.SetProperty(p => p.DateEnd, permission.DateEnd)
					.SetProperty(p => p.Status, permission.Status));

			return new CodeResult { Code = 1 };
		}
	}
}
