namespace Core.Metadata.Options
{
	public interface IActiveUser
	{
		Guid UserId { get; }
		string Email { get; }
		string[] Permissions { get; }
		bool IsAuthenticated { get; }
		bool Invalid { get; }
	}

	public class ActiveUser : IActiveUser
	{
		public Guid UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string[] Permissions { get; set; } = [];
		public bool IsAuthenticated => UserId != Guid.Empty;
		public bool Invalid => UserId == Guid.Empty;
	}
}
