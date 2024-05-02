using MediNet_BE.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models.Clinics
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OpeningHours { get; set; }
        public DateTime ClosingHours { get; set; }
        public string Description { get; set; }
        public string ImagesClinic { get; set; }
        [NotMapped]
		public List<string> ImagesSrc { get; set; } = [];
		public ICollection<Service>? Services { get; set; }
        public ICollection<Supply>? Supplies { get; set; }
		public ICollection<Cart>? Carts { get; }
	}
}
