namespace MediNet_BE.Models.Categories
{
	public class CategoryParent
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public ICollection<Category>? Categories { get; set;}
	}
}
