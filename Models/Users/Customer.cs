using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Products;

namespace MediNet_BE.Models.Users
{
	public class Customer : User
	{
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
		public ICollection<Cart>? Carts { get; set; }
		public ICollection<Order>? Orders { get; set; }
		public ICollection<Feedback>? Feedbacks { get; set; }
		public ICollection<BlogComment>? BlogComments { get; set; }

	}
}
