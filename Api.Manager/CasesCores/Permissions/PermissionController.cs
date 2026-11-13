using Core.Authority.Cases.Permissions;
using Core.Metadata.Options;
using Microsoft.AspNetCore.Mvc;

namespace Api.Manager.CasesCores.Permissions
{
	[ApiController]
	public partial class PermissionController : ControllerBase
	{
		private readonly IActiveUser _activeUser;
		private readonly PermissionCreate _permissionCreate;
		private readonly PermissionChangeTargetName _permissionChangeTargetName;
		private readonly PermissionGet _permissionGet;
		private readonly PermissionModify _permissionModify;
		private readonly PermissionRequest _permissionRequest;

		public PermissionController(
			IActiveUser activeUser,
			PermissionCreate permissionCreate,
			PermissionChangeTargetName permissionChangeTargetName,
			PermissionGet permissionGet,
			PermissionModify permissionModify,
			PermissionRequest permissionRequest)
		{
			this._activeUser = activeUser;
			this._permissionCreate = permissionCreate;
			this._permissionChangeTargetName = permissionChangeTargetName;
			this._permissionGet = permissionGet;
			this._permissionModify = permissionModify;
			this._permissionRequest = permissionRequest;
		}
	}
}
