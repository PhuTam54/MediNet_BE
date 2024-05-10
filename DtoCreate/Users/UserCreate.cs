using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.DtoCreate.Users
{
    public class UserCreate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
