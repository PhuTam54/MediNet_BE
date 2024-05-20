using MediNet_BE.Dto.Employees;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderServices
{
    public class ServiceDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		public int EmployeeId { get; set; }
		public EmployeeDto Employee { get; set; }
		public ICollection<OrderServiceDto>? OrderServices { get; set; }
	}
}
