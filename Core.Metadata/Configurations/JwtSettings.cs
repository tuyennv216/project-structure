namespace Core.Metadata.Configurations
{
	public class JwtSettings
	{
		public string SecretKey { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int TokenExpireMinutes { get; set; } = 15;
		public int RefreshTokenExpireDays { get; set; } = 1;
	}
}
