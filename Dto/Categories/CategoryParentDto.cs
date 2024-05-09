using MediNet_BE.Models.Categories;

namespace MediNet_BE.Dto.Categories
{
	public class CategoryParentDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<CategoryDto>? Categories { get; set; }
	}
}
