using MediNet_BE.Models.Products;

namespace MediNet_BE.Models.Clinics
{
	public class StockOut
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateOut { get; set; }
		public string Reason { get; set; }
		public Clinic Clinic { get; set; }
		public Product Product { get; set; }
	}
}
