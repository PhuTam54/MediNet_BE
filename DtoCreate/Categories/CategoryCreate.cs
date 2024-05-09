namespace MediNet_BE.DtoCreate.Categories
{
    public class CategoryCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryParentId { get; set; }
    }
}
