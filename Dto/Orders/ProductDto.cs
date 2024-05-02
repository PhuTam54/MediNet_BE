using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryChildId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = string.Empty;
		public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
		public string Manufacturer { get; set; }
		public DateTime ManufacturerDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public IFormFile? ImageFile { get; set; }
	}
}
