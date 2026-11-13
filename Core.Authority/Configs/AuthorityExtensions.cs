using Core.Authority.Cases.Permissions;
using Core.Authority.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Authority.Configs
{
	public static class AuthorityServiceExtensions
	{
		public static IServiceCollection AddAuthorityCore(this IServiceCollection services, string connectionString)
		{
			// Đăng ký DbContext
			services.AddDbContext<AuthorityDb>(options => options.UseNpgsql(connectionString));

			// Đăng ký Service nghiệp vụ
			services.AddScoped<PermissionGet>();
			services.AddScoped<PermissionRequest>();

			return services;
		}
	}
}
