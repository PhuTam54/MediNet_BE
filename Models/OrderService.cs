using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models
{
	public class OrderService
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Subtotal { get; set; }
		public int OrderId { get; set; }
		public int ServiceId { get; set; }
		public Order Order { get; set; }
		public Service Service { get; set; }
	}
}
