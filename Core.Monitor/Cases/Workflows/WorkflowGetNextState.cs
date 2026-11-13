namespace Core.Monitor.Cases.Workflows
{
	public class WorkflowGetNextState
	{
		public string GetNextState(string Workflow, string CurrentState)
		{
			var states = Workflow.Split(';');

			if (states.Length == 0) return string.Empty;
			if (string.IsNullOrEmpty(CurrentState)) return states[0];

			var currentIndex = Array.IndexOf(states, CurrentState);
			if (currentIndex < states.Length - 1)
				return states[currentIndex + 1];

			return "Done";
		}
	}
}
