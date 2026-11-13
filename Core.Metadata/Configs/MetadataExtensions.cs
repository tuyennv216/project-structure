using Core.Metadata.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Metadata.Configs
{
	public static class MetadataServiceExtensions
	{
		public static IServiceCollection AddMetadataCore(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
			services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
			services.Configure<GoogleSettings>(configuration.GetSection("Google"));

			return services;
		}
	}
}
