using MediNet_BE.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public int QtyCart { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal SubTotal { get; set; }
		public int UserId { get; set; }
		public int ProductId { get; set; }
		public User User { get; set; }
		public Product Product { get; set; }
	}
}
