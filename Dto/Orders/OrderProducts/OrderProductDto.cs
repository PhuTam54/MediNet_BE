using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class OrderProductDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }
    }
}
