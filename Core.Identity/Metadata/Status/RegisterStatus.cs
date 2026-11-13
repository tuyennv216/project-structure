namespace Core.Identity.Metadata.Status
{
	public static class RegisterStatus
	{
		public const int None = 0;
		public const int Ok = 1;
		public const int EmailAlreadyExists = 2;
		public const int EmailConfirmed = 3;
		public const int InvalidEmailToken = 7;
	}
}
