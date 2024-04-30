using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Dto.Users
{
	public class CustomerDto : UserDto
	{
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public int Gender { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public ICollection<OrderDto>? Orders { get; set; }
		public ICollection<FeedbackDto>? Feedbacks { get; set; }
	}
}
