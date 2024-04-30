namespace MediNet_BE.Models.Users
{
    public class User
    {
		public int Id { get; set; }
		public string Username { get; set; }
		public string SEO_Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int Role { get; set; }
		public int Status { get; set; }
		public string Image { get; set; } = string.Empty;

	}
}
