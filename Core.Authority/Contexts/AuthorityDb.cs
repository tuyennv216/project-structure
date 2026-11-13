using Core.Authority.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Authority.Contexts
{
	public class AuthorityDb : DbContext
	{
		public AuthorityDb(DbContextOptions<AuthorityDb> options) : base(options)
		{
		}

		public DbSet<Permission> Permissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("authority");

			modelBuilder.Entity<Permission>()
				.HasIndex(p => new { p.UserId, p.TargetName });
		}
	}
}
