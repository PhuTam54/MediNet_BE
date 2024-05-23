using MediNet_BE.Dto.Products;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Dto.Clinics
{
    public class StockInDto
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateIn { get; set; }
		public string Supplier { get; set; }
		public DateTime ManufacturerDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public StockInStatus Status { get; set; }
		public ClinicDto Clinic { get; set; }
		public ProductDto Product { get; set; }
	}
}
