using MediNet_BE.Dto.Categories;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.Models.Categories;

namespace MediNet_BE.Interfaces.Categories
{
    public interface ICategoryChildRepo
    {
        public Task<List<CategoryChildDto>> GetAllCategoryChildAsync();
        public Task<CategoryChildDto> GetCategoryChildByIdAsync(int id);
        public Task<CategoryChild> AddCategoryChildAsync(CategoryChildCreate categoryChildCreate);
        public Task UpdateCategoryChildAsync(CategoryChildCreate categoryChildCreate);
        public Task DeleteCategoryChildAsync(int id);
    }
}
