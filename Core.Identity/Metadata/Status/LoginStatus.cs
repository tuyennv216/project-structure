namespace Core.Identity.Metadata.Status
{
	public static class LoginStatus
	{
		public const int None = 0;
		public const int Ok = 1;
		public const int NeedConfirmEmail = 2;
		public const int NotFound = 5;
		public const int NeedPasscode = 6;
		public const int NeedUpdatePassword = 20;
		public const int LoggedOut = 22;
		public const int TokenExpired = 23;
		public const int Locked = 24;
		public const int Disabled = 25;
		public const int InvalidDevice = 27;
		public const int ForgotPassword = 45;
	}
}
