using Core.History.Cases;
using Core.History.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.History.Configs
{
	public static class HistoryServiceExtensions
	{
		public static IServiceCollection AddHistoryCore(this IServiceCollection services, string connectionString)
		{
			// Đăng ký DbContext
			services.AddDbContext<HistoryDb>(options => options.UseNpgsql(connectionString));

			// Đăng ký Service nghiệp vụ
			services.AddScoped<HistoryTakeSnapshot>();
			services.AddScoped<HistoryReadPagination>();

			return services;
		}
	}
}
