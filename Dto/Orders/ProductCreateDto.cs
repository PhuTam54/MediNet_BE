using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders
{
    public class ProductCreateDto
    {
        public int Id { get; set; }
        public int CategoryChildId { get; set; }
        public int ClinicId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ManufacturerDate { get; set; }
        public DateTime? ExpiryDate { get; set; } = null;
        public IFormFile? ImageFile { get; set; }
    }
}
