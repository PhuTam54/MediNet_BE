namespace MediNet_BE.DtoCreate.Orders.OrderProducts
{
    public class CartCreate
    {
        public int Id { get; set; }
        public int QtyCart { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int ClinicId { get; set; }
    }
}
