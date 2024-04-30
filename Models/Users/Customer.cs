using MediNet_BE.Models.Orders;

namespace MediNet_BE.Models.Users
{
    public class Customer : User
	{
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public ICollection<Order>? Orders { get;}
		public ICollection<Feedback>? Feedbacks { get; }
	}
}
