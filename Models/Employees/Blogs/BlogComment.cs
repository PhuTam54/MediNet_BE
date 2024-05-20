using MediNet_BE.Models.Users;

namespace MediNet_BE.Models.Employees.Blogs
{
	public class BlogComment
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int BlogId { get; set; }
		public int Parent_Id { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdatedAt { get; set; }
		public Customer Customer { get; set; }
		public Blog Blog { get; set; }
	}
}
