using Core.Monitor.Cases.Monitors;
using Core.Monitor.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Monitor.Configs
{
	public static class MonitorServiceExtensions
	{
		public static IServiceCollection AddMonitorCore(this IServiceCollection services, string connectionString)
		{
			// Đăng ký DbContext
			services.AddDbContext<MonitorDb>(options => options.UseNpgsql(connectionString));

			// Đăng ký Service nghiệp vụ
			services.AddScoped<MonitorNewFlowLog>();
			services.AddScoped<MonitorNewFlowTrace>();
			services.AddScoped<MonitorSaveFlowState>();

			return services;
		}
	}
}
