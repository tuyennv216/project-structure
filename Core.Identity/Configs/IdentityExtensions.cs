using Core.Identity.Cases.Identities;
using Core.Identity.Cases.IdentitiesExternal;
using Core.Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Identity.Configs
{
	public static class IdentityServiceExtensions
	{
		public static IServiceCollection AddIdentityCore(this IServiceCollection services, string connectionString)
		{
			// Đăng ký DbContext
			services.AddDbContext<IdentityDb>(options => options.UseNpgsql(connectionString));

			// Đăng ký Service nghiệp vụ
			services.AddScoped<IdentityConfirmEmail>();
			services.AddScoped<IdentityForgotPassword>();
			services.AddScoped<IdentityLogout>();
			services.AddScoped<IdentityLogin>();
			services.AddScoped<IdentityRefreshToken>();
			services.AddScoped<IdentityRegister>();
			services.AddScoped<IdentityResetPassword>();

			services.AddScoped<ExternalLoginGoogle>();

			return services;
		}
	}
}
