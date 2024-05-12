namespace MediNet_BE.DtoCreate.Orders
{
    public class OrderCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string District { get; set; }
		public string Zip_Code { get; set; }
		public string Description { get; set; }
        public string Shipping_method { get; set; }
        public string Payment_method { get; set; }
        public int CustomerId { get; set; }
        public List<int> CartIds { get; set; }
    }
}
