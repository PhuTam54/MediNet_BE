using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;

namespace MediNet_BE.Interfaces.Employees.Blogs
{
    public interface IBlogRepo
    {
        public Task<List<BlogDto>> GetAllBlogAsync();
        public Task<BlogDto> GetBlogByIdAsync(int id);
		public Task<List<BlogDto>> GetBlogsByEmployeeIdAsync(int employeeId);
		public Task<List<BlogDto>> GetBlogsByDiseaseIdAsync(int diseaseId);
		public Task<Blog> AddBlogAsync(BlogCreate blogCreate);
        public Task UpdateBlogAsync(BlogCreate blogCreate);
        public Task DeleteBlogAsync(int id);
    }
}
