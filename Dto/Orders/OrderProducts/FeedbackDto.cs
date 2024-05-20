using MediNet_BE.Dto.Users;
using MediNet_BE.Dto.Products;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class FeedbackDto
    {
		public int Id { get; set; }
		public int Vote { get; set; }
		public string ImagesFeedback { get; set; }
		public List<string> ImagesSrc { get; set; } = [];
		public string Description { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public CustomerDto Customer { get; set; }
		public ProductDto Product { get; set; }
	}
}
