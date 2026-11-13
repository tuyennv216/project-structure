namespace Core.Authority.Metadata
{
	public static class PermissionStatus
	{
		public const int None = 0;
		public const int Ok = 1;
		public const int Disabled = 5;
		public static readonly int[] All = { None, Ok, Disabled };
	}
}
