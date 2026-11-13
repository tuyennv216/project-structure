namespace Core.Monitor.Entities
{
	public class WorkTrace
	{
		public Guid RequestId { get; set; }
		public required string State { get; set; }
		public string? Content { get; set; }
		public string? Log { get; set; }
	}
}
