using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models
{
	public class Service
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		public int ClinicId { get; set; }
		public Clinic Clinic { get; set; }
		public ICollection<OrderService>? OrderServices { get; set; }
	}
}
