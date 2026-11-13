using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pack.Information.Cases.Customers;
using Pack.Information.Cases.Workers;
using Pack.Information.Cases.Workshops;
using Pack.Information.Contexts;

namespace Pack.Information.Configs
{
	public static class InfoServiceExtensions
	{
		public static IServiceCollection AddInfoPackage(this IServiceCollection services, string connectionString)
		{
			// Đăng ký DbContext
			services.AddDbContext<InfoDb>(options => options.UseNpgsql(connectionString));

			// Đăng ký Service nghiệp vụ
			services.AddScoped<CustomerApprove>();
			services.AddScoped<CustomerCreate>();
			services.AddScoped<CustomerModify>();
			services.AddScoped<CustomerRead>();
			services.AddScoped<CustomerReadPagination>();

			services.AddScoped<WorkerApprove>();
			services.AddScoped<WorkerCreate>();
			services.AddScoped<WorkerModify>();
			services.AddScoped<WorkerRead>();
			services.AddScoped<WorkerReadPagination>();

			services.AddScoped<WorkshopApprove>();
			services.AddScoped<WorkshopCreate>();
			services.AddScoped<WorkshopModify>();
			services.AddScoped<WorkshopRead>();
			services.AddScoped<WorkshopReadPagination>();

			return services;
		}
	}
}
