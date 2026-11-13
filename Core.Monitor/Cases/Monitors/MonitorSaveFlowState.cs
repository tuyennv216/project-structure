using Core.Monitor.Contexts;

namespace Core.Monitor.Cases.Monitors
{
	public class MonitorSaveFlowState
	{
		private readonly MonitorDb _monitorDb;

		public MonitorSaveFlowState(MonitorDb monitorDb)
		{
			_monitorDb = monitorDb;
		}

		public async Task<bool> SaveFlowState(Guid RequestId, string NewState)
		{
			var flowState = await _monitorDb.FlowStates.FindAsync(RequestId);

			if (flowState == null) return false;

			flowState.CurrentState = NewState;

			_monitorDb.FlowStates.Update(flowState);

			await _monitorDb.SaveChangesAsync();

			return true;
		}
	}
}
