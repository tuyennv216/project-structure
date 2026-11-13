using Core.Monitor.Contexts;

namespace Core.Monitor.Cases.Monitors
{
	public class MonitorNewFlowLog
	{
		private readonly MonitorDb _monitorDb;

		public MonitorNewFlowLog(MonitorDb monitorDb)
		{
			_monitorDb = monitorDb;
		}

		public async Task<bool> NewTraceLog(Guid RequestId, string State, string Log)
		{
			var trace = new Entities.WorkTrace
			{
				RequestId = RequestId,
				State = State,
				Log = Log,
			};

			await _monitorDb.FlowTraces.AddAsync(trace);
			await _monitorDb.SaveChangesAsync();

			return true;
		}
	}
}
