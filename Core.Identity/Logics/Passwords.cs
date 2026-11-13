using System.Security.Cryptography;
using System.Text;

namespace Core.Identity.Logics
{
	internal static class Passwords
	{
		private const int SaltSize = 16;	// Kích thước (128-bit)
		private const int HashSize = 32;	// Kích thước (256-bit)
		private const int Iterations = 100000;


		// Kiểm tra mã hóa mật khẩu với Shalt[cite: 2]
		internal static bool VerifyPassword(string password, string? salt, string? storedHash)
		{
			if (string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(storedHash))
			{
				return false;
			}

			using var sha256 = SHA256.Create();
			var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
			var hashBytes = sha256.ComputeHash(combinedBytes);
			string computedHash = Convert.ToBase64String(hashBytes);

			return computedHash == storedHash;
		}

		/// <summary>
		/// Tạo một chuỗi Salt ngẫu nhiên dạng Base64.
		/// </summary>
		internal static string GenerateSalt()
		{
			byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
			return Convert.ToBase64String(saltBytes);
		}

		/// <summary>
		/// Mã hóa mật khẩu kết hợp với Salt sử dụng PBKDF2.
		/// </summary>
		internal static string HashPassword(string password, string base64Salt)
		{
			if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(base64Salt))
			{
				return string.Empty;
			}

			byte[] saltBytes = Convert.FromBase64String(base64Salt);

			byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
				password,
				saltBytes,
				Iterations,
				HashAlgorithmName.SHA256,
				HashSize);

			return Convert.ToBase64String(hashBytes);
		}
	}
}
