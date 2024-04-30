using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Models
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
		public int StockQuantity { get; set; }
		public string Manufacturer { get; set; }
		public DateTime ManufacturerDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public int CategoryChildId { get; set; }
		public int ClinicId { get; set; }
		public CategoryChild CategoryChild { get; set; }
		public Clinic Clinic { get; set; }
		public ICollection<OrderProduct>? OrderProducts { get; set; }
		public ICollection<Feedback>? Feedbacks { get; }

	}
}
