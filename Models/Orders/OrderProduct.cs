using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Models.Products;

namespace MediNet_BE.Models.Orders
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
