using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Dto.Products;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Products;

namespace MediNet_BE.Dto.Users
{
	public class CustomerDto : UserDto
	{
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public ICollection<CartDto>? Carts { get; set; }
		public ICollection<OrderDto>? Orders { get; set; }
		public ICollection<FeedbackDto>? Feedbacks { get; set; }
		public ICollection<FavoriteProductDto>? FavoriteProducts { get; set; }
		public ICollection<BlogCommentDto>? BlogComments { get; set; }
	}
}
