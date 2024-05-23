using MediNet_BE.Models.Clinics;

namespace MediNet_BE.DtoCreate.Clinics
{
	public class StockInCreate
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
	}
}
