namespace Core.Identity.Entities
{
	public class User
	{
		public Guid UserId { get; set; }
		public required string Email { get; set; }
		public string? Username { get; set; }
		public string? Fullname { get; set; }
		public string? AvatarUrl { get; set; }
		public string? GoogleId { get; set; }
		public Guid? GroupId { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateEnd { get; set; }
		public int Status { get; set; }
		public bool Confirmed { get; set; }
		public int Pow { get; set; }
	}
}
