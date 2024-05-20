using MediNet_BE.Models.Products;

namespace MediNet_BE.Models.Clinics
{
	public class StockIn
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateIn { get; set; }
		public string Supplier {  get; set; }
		public DateTime ManufacturerDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public int Status { get; set; }
		public Clinic Clinic { get; set; }
		public Product Product { get; set; }
	}
}
