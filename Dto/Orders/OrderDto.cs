using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders
{
    public class OrderDto
    {
		public int Id { get; set; }
		public string OrderCode { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Tel { get; set; }
		public string Address { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string District { get; set; }
		public string Zip_Code { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal TotalAmount { get; set; }
		public string Shipping_method { get; set; }
		public string Payment_method { get; set; }
		public bool Is_paid { get; set; }
		public DateTime OrderDate { get; set; }
		public OrderStatus Status { get; set; }
		public int CustomerId { get; set; }
		public CustomerDto Customer { get; set; }
		public ICollection<OrderProductDto>? OrderProducts { get; set; }
		public ICollection<OrderServiceDto>? OrderServices { get; set; }
	}
}
