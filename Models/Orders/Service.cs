using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Models.Doctors;
namespace MediNet_BE.Models.Orders
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<OrderService>? OrderServices { get; set; }
    }
}
