namespace MediNet_BE.Dto.Users
{
    public enum Role
    {
        ADMIN, DOCTOR, PATIENT
    }
    public class UserDto
    {
        public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
        public string Password { get; set; }
		public Role Role { get; set; }
		public int Gender { get; set; }
		public string Image { get; set; } = string.Empty;
		public IFormFile? ImageFile { get; set; }
	}
}