namespace MediNet_BE.Dto.Clinics
{
	public class SupplyDto
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int StockQuantity { get; set; }
	}
}
