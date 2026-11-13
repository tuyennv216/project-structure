using Core.Monitor.Contexts;

namespace Core.Monitor.Cases.Monitors
{
	public class MonitorNewFlowTrace
	{
		private readonly MonitorDb _monitorDb;

		public MonitorNewFlowTrace(MonitorDb monitorDb)
		{
			_monitorDb = monitorDb;
		}

		public async Task<bool> NewTraceFlow(Guid RequestId, string State, string Content)
		{
			var trace = new Entities.WorkTrace
			{
				RequestId = RequestId,
				State = State,
				Content = Content
			};

			await _monitorDb.FlowTraces.AddAsync(trace);
			await _monitorDb.SaveChangesAsync();

			return true;
		}
	}
}
