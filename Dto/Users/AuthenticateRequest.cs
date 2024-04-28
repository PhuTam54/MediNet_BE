using System.ComponentModel.DataAnnotations;

namespace MediNet_BE.Dto.Users
{
	public class AuthenticateRequest
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
