using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.DtoCreate.Orders.OrderServices
{
    public class ServiceCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int EmployeeId { get; set; }
    }
}
