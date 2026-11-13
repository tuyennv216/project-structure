using Core.History.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.History.Contexts
{
	public class HistoryDb : DbContext
	{
		public HistoryDb(DbContextOptions<HistoryDb> options) : base(options)
		{
		}

		public DbSet<Snapshot> Snapshots { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("history");

			modelBuilder.Entity<Snapshot>()
				.HasIndex(s => new { s.UserId, s.TargetName });
		}
	}
}
