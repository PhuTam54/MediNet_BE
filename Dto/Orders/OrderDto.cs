using MediNet_BE.Dto.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
        public string Shipping_method { get; set; }
        public string Payment_method { get; set; }
        public bool Is_paid { get; set; } = false;
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<CartItem>? CartList { get; set; }
		public CustomerDto? Customer { get; set; }
		public ICollection<OrderProductDto>? OrderProducts { get; set; }
		public ICollection<OrderServiceDto>? OrderServices { get; set; }
	}
}
