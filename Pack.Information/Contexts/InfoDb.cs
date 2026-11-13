using Microsoft.EntityFrameworkCore;
using Pack.Information.Entities;

namespace Pack.Information.Contexts
{
	public class InfoDb : DbContext
	{
		public InfoDb(DbContextOptions<InfoDb> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Worker> Workers { get; set; }
		public DbSet<Workshop> Workshops { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("info");
		}
	}
}
