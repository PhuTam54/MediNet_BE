using System.ComponentModel.DataAnnotations;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models;
using MediNet_BE.Dto.Orders.OrderProducts;

namespace MediNet_BE.Dto.Categories
{
    public class CategoryChildDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public CategoryDto Category { get; set; }
		public ICollection<ProductDto>? Products { get; set; }
	}
}
