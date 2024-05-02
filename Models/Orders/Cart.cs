using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models.Orders
{
	public class Cart
	{
		public int Id { get; set; }
		public int QtyCart { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal SubTotal { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public int ClinicId { get; set; }
		public Customer Customer { get; set; }
		public Product Product { get; set; }
		public Clinic Clinic { get; set; }
	}
}
