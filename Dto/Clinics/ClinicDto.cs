using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Dto.Clinics
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
    }
}
