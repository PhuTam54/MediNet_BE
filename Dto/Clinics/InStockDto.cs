using MediNet_BE.Dto.Products;

namespace MediNet_BE.Dto.Clinics
{
    public class InStockDto
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int StockQuantity { get; set; }
		public int QuantitySold { get; set; }
		public DateTime LastUpdatedAt { get; set; }
		public ClinicDto Clinic { get; set; }
		public ProductDto Product { get; set; }
	}
}
