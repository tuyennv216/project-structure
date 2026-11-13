using Core.Monitor.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Monitor.Contexts
{
	public class MonitorDb : DbContext
	{
		public MonitorDb(DbContextOptions<MonitorDb> options) : base(options)
		{
		}

		public DbSet<Workflow> Workflows { get; set; }
		public DbSet<WorkState> FlowStates { get; set; }
		public DbSet<WorkTrace> FlowTraces { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("monitor");
		}
	}
}
