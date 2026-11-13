namespace Core.Authority.Entities
{
	public class Permission
	{
		public Guid UserId { get; set; }
		public required string TargetName { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateEnd { get; set; }
		public int Status { get; set; }
	}
}
