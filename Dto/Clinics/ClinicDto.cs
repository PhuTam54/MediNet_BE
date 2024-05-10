using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Clinics
{
    public class ClinicDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public DateTime OpeningHours { get; set; }
		public DateTime ClosingHours { get; set; }
		public string Description { get; set; }
		public string ImagesClinic { get; set; }
		public List<string> ImagesSrc { get; set; } = [];
		public ICollection<SupplyDto>? Supplies { get; set; }
		public ICollection<CartDto>? Carts { get; set; }
	}
}
