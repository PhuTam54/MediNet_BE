namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class CartDto
    {
        public int Id { get; set; }
        public int QtyCart { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int ClinicID { get; set; }
    }
}
