using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediNet_BE.Dto.Orders
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
        public decimal SubTotal => Math.Round(Price * Qty, 2);
    }
}
