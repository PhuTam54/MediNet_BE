using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.Dto.Products;

namespace MediNet_BE.Dto.Users
{
	public class CustomerDto : UserDto
	{
		public ICollection<FavoriteProductDto>? FavoriteProducts { get; set; }
		public ICollection<BlogCommentDto>? BlogComments { get; set; }
	}
}
