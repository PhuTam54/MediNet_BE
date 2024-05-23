using MediNet_BE.Dto.Products;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Dto.Clinics
{
    public class StockOutDto
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateOut { get; set; }
		public StockOutReason Reason { get; set; }
		public ClinicDto Clinic { get; set; }
		public ProductDto Product { get; set; }
	}
}
