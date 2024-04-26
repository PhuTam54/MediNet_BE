namespace MediNet_BE.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<CategoryChild>? CategoryChilds { get; set; }

	}
}
