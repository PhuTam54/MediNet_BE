using MediNet_BE.Dto.Users;

namespace MediNet_BE.Dto.Employees.Blogs
{
	public class BlogCommentDto
	{
		public int Id { get; set; }
		public int Parent_Id { get; set; } = 0;
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdatedAt { get; set; }
		public int CustomerId { get; set; }
		public int BlogId { get; set; }
		public CustomerDto Customer { get; set; }
		public BlogDto Blog { get; set; }
	}
}
