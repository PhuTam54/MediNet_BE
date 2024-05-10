using MediNet_BE.Dto.Courses;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderServices
{
    public class ServiceDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		public int DoctorId { get; set; }
		public DoctorDto Doctor { get; set; }
		public ICollection<OrderServiceDto>? OrderServices { get; set; }
	}
}
