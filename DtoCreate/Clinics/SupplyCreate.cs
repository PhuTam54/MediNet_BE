namespace MediNet_BE.DtoCreate.Clinics
{
    public class SupplyCreate
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
    }
}
