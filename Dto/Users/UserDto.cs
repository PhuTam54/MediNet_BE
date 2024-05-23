using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Users
{
	public class UserDto
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public int Role { get; set; }
		public int Status { get; set; }
		public string Image { get; set; } = string.Empty;
		public string ImageSrc { get; set; } = string.Empty;
	}
}
