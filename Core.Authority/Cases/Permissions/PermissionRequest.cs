namespace Core.Authority.Cases.Permissions
{
	public class PermissionRequest
	{
		public PermissionRequest()
		{
		}

		public bool Request(string[] permissions, params string[] names)
		{
			var permissionName = string.Join("_", names);
			return Array.IndexOf(permissions, permissionName) >= 0;
		}
	}
}
