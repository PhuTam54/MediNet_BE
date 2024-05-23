using MediNet_BE.Models.Products;

namespace MediNet_BE.Models.Clinics
{
	public enum StockOutReason
	{
		SALE, CANCEL, MOVING_STORE, EXPIRED
	}
	public class StockOut
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateOut { get; set; }
		public StockOutReason Reason { get; set; }
		public Clinic Clinic { get; set; }
		public Product Product { get; set; }
	}
}
