using MediNet_BE.Dto.Users;

namespace MediNet_BE.Dto.Products
{
	public class FavoriteProductDto
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public DateTime CreatedAt { get; set; }
		public CustomerDto Customer { get; set; }
		public ProductDto Product { get; set; }
	}
}
