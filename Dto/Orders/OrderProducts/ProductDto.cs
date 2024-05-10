using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderProducts
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
		public ICollection<SupplyDto>? Supplies { get; set; }
		public ICollection<CartDto>? Carts { get; set; }
		public ICollection<OrderProductDto>? OrderProducts { get; set; }
		public ICollection<FeedbackDto>? Feedbacks { get; set; }
	}
}
