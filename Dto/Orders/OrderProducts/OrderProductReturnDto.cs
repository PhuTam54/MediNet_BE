using MediNet_BE.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class OrderProductReturnDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public ProductDto Product { get; set; }
    }
}
