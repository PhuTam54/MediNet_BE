using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto
{
	public class OrderServiceDto
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Subtotal { get; set; }
	}
}
