namespace MediNet_BE.DtoCreate.Employees.Blogs
{
	public class BlogCommentCreate
	{
		public int Id { get; set; }
		public int Parent_Id { get; set; } = 0;
		public string Comment { get; set; }
		public int CustomerId { get; set; }
		public int BlogId { get; set; }
	}
}
