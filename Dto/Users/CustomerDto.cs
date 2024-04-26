namespace MediNet_BE.Dto.Users
{
	public class CustomerDto : UserDto
	{
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		
	}
}
