using Core.Authority.Cases.Permissions;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	[ApiController]
	public partial class PermissionGroupController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly PermissionGroupCreate _permissionGroupCreate;
		private readonly PermissionGroupModify _permissionGroupModify;

		public PermissionGroupController(
			IActiveUser activeUser,
			PermissionGroupCreate permissionGroupCreate,
			PermissionGroupModify permissionGroupModify)
		{
			this._activeUser = activeUser;
			this._permissionGroupCreate = permissionGroupCreate;
			this._permissionGroupModify = permissionGroupModify;
		}
	}
}
