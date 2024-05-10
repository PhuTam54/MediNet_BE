using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderServices
{
    public class OrderServiceDto
    {
		public int Id { get; set; }
		public int Quantity { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Subtotal { get; set; }
		public int OrderId { get; set; }
		public int ServiceId { get; set; }
		public OrderDto Order { get; set; }
		public ServiceDto Service { get; set; }
	}
}
