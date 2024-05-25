using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;

namespace MediNet_BE.Interfaces.Employees.Blogs
{
	public interface IBlogCommentRepo
	{
		public Task<List<BlogCommentDto>> GetAllBlogCommentAsync();
		public Task<List<BlogCommentDto>> GetBlogCommentByParentIdAsync(int parentId);
		public Task<BlogCommentDto> GetBlogCommentByIdAsync(int id);
		public Task<List<BlogCommentDto>> GetBlogCommentByBlogIdAsync(int blogId);
		public Task<BlogComment> AddBlogCommentAsync(BlogCommentCreate blogCommentCreate);
		public Task UpdateBlogCommentAsync(BlogCommentCreate blogCommentCreate);
		public Task DeleteBlogCommentAsync(int id);
	}
}
