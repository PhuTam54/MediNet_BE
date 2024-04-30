using MediNet_BE.Dto.Orders;
using MediNet_BE.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto
{
	public class ServiceDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		public int ClinicId { get; set; }
		public Clinic? Clinic { get; set; }
		public ICollection<OrderServiceDto>? OrderServices { get; set; }
	}
}
