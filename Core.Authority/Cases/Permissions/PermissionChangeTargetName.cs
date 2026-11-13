using Core.Authority.Contexts;
using Core.Metadata.Names;
using Core.Metadata.Options;
using Core.Metadata.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionChangeTargetName
	{
		private readonly AuthorityDb _authorityDb;
		private readonly PermissionRequest _permissionRequest;
		private readonly IActiveUser _activeUser;

		public PermissionChangeTargetName(AuthorityDb authorityDb, PermissionRequest permissionRequest, IActiveUser activeUser)
		{
			this._authorityDb = authorityDb;
			this._permissionRequest = permissionRequest;
			this._activeUser = activeUser;
		}

		public async Task<CodeResult> ChangeTargetNameAsync(string oldName, string newName)
		{
			var isAdmin = _permissionRequest.Request(_activeUser.Permissions, RoleNames.Admin);
			var canModify = _permissionRequest.Request(_activeUser.Permissions, AccessNames.Modify, ComponentNames.Authority);
			if (!isAdmin || !canModify) return new CodeResult { Code = 0, Error = "Unauthorized" };

			var oldCount = await _authorityDb.Permissions.CountAsync(p => p.TargetName == oldName);
			var newCount = await _authorityDb.Permissions.CountAsync(p => p.TargetName == newName);

			if (oldCount == 0) return new CodeResult { Code = 0, Error = "No data" };
			if (newCount != 0) return new CodeResult { Code = 0, Error = "New name not valid" };

			await _authorityDb.Permissions
				.Where(p => p.TargetName == oldName)
				.ExecuteUpdateAsync(s => s.SetProperty(p => p.TargetName, newName));

			return new CodeResult { Code = 1 };
		}
	}
}
