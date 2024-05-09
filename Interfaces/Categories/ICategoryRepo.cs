using MediNet_BE.Dto.Categories;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.Models.Categories;

namespace MediNet_BE.Interfaces.Categories
{
    public interface ICategoryRepo
    {
        public Task<List<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);
        public Task<Category> AddCategoryAsync(CategoryCreate categoryCreate);
        public Task UpdateCategoryAsync(CategoryCreate categoryCreate);
        public Task DeleteCategoryAsync(int id);
    }
}
