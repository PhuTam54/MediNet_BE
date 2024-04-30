
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Models
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
		public ICollection<Service>? Services { get; set; }
		public ICollection<Product>? Products { get; set; }
	}
}
