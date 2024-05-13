using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Models.Employees;
namespace MediNet_BE.Models.Orders
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public ICollection<OrderService>? OrderServices { get; set; }
    }
}
