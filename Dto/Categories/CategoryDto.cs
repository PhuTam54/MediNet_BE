namespace MediNet_BE.Dto.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public ICollection<CategoryChildDto>? CategoryChilds { get; set; }

	}
}
