namespace Core.Identity.Entities
{
	public class Login
	{
		public required string Email { get; set; }
		public string? Salt { get; set; }
		public string? PasswordHash { get; set; }
		public string? Token { get; set; }
		public string? RefreshToken { get; set; }
		public string? ResetPasswordToken { get; set; }
		public string? EmailToken { get; set; }
		public string? Passcode { get; set; }
		public string? Device { get; set; }
		public int FailedCount { get; set; }
		public bool EnablePasscode { get; set; }
		public int Status { get; set; }
	}
}
