using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Models.Products
{
	public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public int CategoryChildId { get; set; }
        public CategoryChild CategoryChild { get; set; }
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }
        public ICollection<ProductDetail>? ProductDetails { get; set; }
        public ICollection<InStock>? InStocks { get; set; }
		public ICollection<StockIn>? StockIns { get; set; }
		public ICollection<StockOut>? StockOuts { get; set; }
		public ICollection<Cart>? Carts { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
