namespace Core.History.Entities
{
	public class Snapshot
	{
		public Guid UserId { get; set; }
		public required string TargetName { get; set; }
		public string? Data { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
