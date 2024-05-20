using MediNet_BE.Dto.Products;

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
