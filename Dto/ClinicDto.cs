using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Dto
{
    public class ClinicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
		public DateTime OpeningHours { get; set; }
		public DateTime ClosingHours { get; set; }
		public string Description { get; set; }
		public string ImagesClinic { get; set; } = string.Empty;
        public IFormFile[] ImagesClinicFile { get; set; }
		public List<string> ImagesSrc { get; set; } = [];

		public ICollection<ServiceDto>? Services { get; set; }
        public ICollection<ProductDto>? Products { get; set; }
    }
}
