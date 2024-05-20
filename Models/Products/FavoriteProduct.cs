using MediNet_BE.Models.Users;

namespace MediNet_BE.Models.Products
{
	public class FavoriteProduct
	{
		public int Id { get; set; }	
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public DateTime CreatedAt { get; set; }
		public Customer Customer { get; set; }
		public Product Product { get; set; }
	}
}
