using Core.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Identity.Contexts
{
	public class IdentityDb : DbContext
	{
		public IdentityDb(DbContextOptions<IdentityDb> options) : base(options)
		{
		}

		public DbSet<Group> Groups { get; set; }
		public DbSet<Login> Logins { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("identity");
		}
	}
}
