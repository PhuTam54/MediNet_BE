using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Orders;

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
        public ICollection<InStock>? InStocks { get; set; }
		public ICollection<StockIn>? StockIns { get; set; }
		public ICollection<StockOut>? StockOuts { get; set; }
		public ICollection<Cart>? Carts { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
