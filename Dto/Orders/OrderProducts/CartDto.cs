using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Dto.Users;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Products;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class CartDto
    {
		public int Id { get; set; }
		public int QtyCart { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal SubTotal { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public int ClinicId { get; set; }
		public CustomerDto Customer { get; set; }
		public ProductDto Product { get; set; }
		public ClinicDto Clinic { get; set; }
	}
}
