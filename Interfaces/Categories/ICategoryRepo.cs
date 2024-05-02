using MediNet_BE.Dto.Categories;
using MediNet_BE.Models.Categories;

namespace MediNet_BE.Interfaces.Categories
{
    public interface ICategoryRepo
    {
        public Task<List<Category>> GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<Category> AddCategoryAsync(CategoryDto categoryDto);
        public Task UpdateCategoryAsync(CategoryDto categoryDto);
        public Task DeleteCategoryAsync(int id);
    }
}
