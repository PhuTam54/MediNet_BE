namespace MediNet_BE.DtoCreate.Orders
{
    public class OrderCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Shipping_method { get; set; }
        public string Payment_method { get; set; }
        public int CustomerId { get; set; }
        public List<int> CartIds { get; set; }
    }
}
