using MediNet_BE.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models
{
	
	public class Order
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Tel { get; set; }
		public string Address { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal TotalAmount { get; set; }
		public string Shipping_method { get; set; }
		public string Payment_method { get; set; }
		public bool Is_paid { get; set; }
		public DateTime OrderDate { get; set; }
		public int Status { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public ICollection<OrderProduct>? OrderProducts { get; set; }
		public ICollection<OrderService>? OrderServices { get; set; }

	}
}
