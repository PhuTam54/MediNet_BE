using MediNet_BE.Models.Clinics;
using MediNet_BE.Models;
using MediNet_BE.Dto.Orders.OrderProducts;

namespace MediNet_BE.Dto.Clinics
{
	public class SupplyDto
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int StockQuantity { get; set; }
		public ClinicDto Clinic { get; set; }
		public ProductDto Product { get; set; }
	}
}
