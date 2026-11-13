namespace Core.Monitor.Cases.Workflows
{
	internal class WorkflowNeedState
	{
		public bool NeedState(string Workflow, string CurrentState, string TargetState)
		{
			if (string.IsNullOrEmpty(CurrentState))
				return true;

			return Workflow.IndexOf(CurrentState) < Workflow.IndexOf(TargetState);
		}
	}
}
