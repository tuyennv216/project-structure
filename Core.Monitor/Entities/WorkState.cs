namespace Core.Monitor.Entities
{
	public class WorkState
	{
		public Guid RequestId { get; set; }
		public string WorkflowName { get; set; }
		public string CurrentState { get; set; }
	}
}
