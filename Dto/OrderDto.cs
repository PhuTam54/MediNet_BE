using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto
{
	public enum OrderStatus
	{
		PENDING, CONFIRMED, SHIPPING, SHIPPED, COMPLETE, CANCEL
	}
	public class OrderDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Tel { get; set; }
		public string Address { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public string Shipping_method { get; set; }
		public string Payment_method { get; set; }
		public bool Is_paid { get; set; } = false;
		public DateTime OrderDate { get; set; }
		public OrderStatus Status { get; set; }
		public int UserId { get; set; }
		public List<int> CartIds { get; set; }
	}
}
