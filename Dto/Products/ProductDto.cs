using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Orders.OrderProducts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public string ImageSrc { get; set; } = string.Empty;
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ManufacturerDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CategoryChildId { get; set; }
        public CategoryChildDto CategoryChild { get; set; }
        public ICollection<ProductDetailDto>? ProductDetails { get; set; }
        public ICollection<InStockDto>? InStocks { get; set; }
		public ICollection<StockInDto>? StockIns { get; set; }
		public ICollection<StockOutDto>? StockOuts { get; set; }
		public ICollection<CartDto>? Carts { get; set; }
        public ICollection<OrderProductDto>? OrderProducts { get; set; }
        public ICollection<FeedbackDto>? Feedbacks { get; set; }
		public ICollection<FavoriteProductDto>? FavoriteProducts { get; set; }
	}
}
