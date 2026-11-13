namespace Pack.Information.Metadata
{
	public static class WorkerStatus
	{
		public const int None = 0;
		public const int New = 1;
		public const int Modified = 2;
		public const int Ok = 3;

		public static int[] All = { None, New, Modified, Ok };
	}
}
