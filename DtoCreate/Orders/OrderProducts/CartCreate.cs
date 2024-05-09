namespace MediNet_BE.DtoCreate.Orders.OrderProducts
{
    public class CartCreate
    {
        public int Id { get; set; }
        public int QtyCart { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int ClinicID { get; set; }
    }
}
