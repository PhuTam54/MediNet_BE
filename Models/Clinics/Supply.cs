namespace MediNet_BE.Models.Clinics
{
    public class Supply
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public Clinic Clinic { get; set; }
        public Product Product { get; set; }
    }
}
