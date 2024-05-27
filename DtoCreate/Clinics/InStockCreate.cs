namespace MediNet_BE.DtoCreate.Clinics
{
    public class InStockCreate
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
		public int QuantitySold { get; set; }
	}
}
