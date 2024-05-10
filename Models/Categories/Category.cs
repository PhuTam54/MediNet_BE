namespace MediNet_BE.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int CategoryParentId { get; set; }
        public CategoryParent CategoryParent { get; set; }
        public ICollection<CategoryChild>? CategoryChilds { get; set; }
    }
}
