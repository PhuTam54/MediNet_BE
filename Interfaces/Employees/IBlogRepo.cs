using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Models.Employees;

namespace MediNet_BE.Interfaces.Employees
{
    public interface IBlogRepo
    {
        public Task<List<BlogDto>> GetAllBlogAsync();
        public Task<BlogDto> GetBlogByIdAsync(int id);
        public Task<Blog> AddBlogAsync(BlogCreate blogCreate);
        public Task UpdateBlogAsync(BlogCreate blogCreate);
        public Task DeleteBlogAsync(int id);
    }
}
