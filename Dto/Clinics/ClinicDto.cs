using MediNet_BE.Dto.Employees;
using MediNet_BE.Dto.Orders.OrderProducts;

namespace MediNet_BE.Dto.Clinics
{
    public class ClinicDto
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
		public List<string> ImagesSrc { get; set; } = [];
		public ICollection<InStockDto>? InStocks { get; set; }
		public ICollection<StockInDto>? StockIns { get; set; }
		public ICollection<StockOutDto>? StockOuts { get; set; }
		public ICollection<CartDto>? Carts { get; set; }
		public ICollection<EmployeeDto>? Employees { get; set; }

	}
}
