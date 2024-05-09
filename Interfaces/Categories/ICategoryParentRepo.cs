using MediNet_BE.Dto.Categories;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.Models.Categories;

namespace MediNet_BE.Interfaces.Categories
{
    public interface ICategoryParentRepo
	{
		public Task<List<CategoryParentDto>> GetAllCategoryParentAsync();
		public Task<CategoryParentDto> GetCategoryParentByIdAsync(int id);
		public Task<CategoryParent> AddCategoryParentAsync(CategoryParentCreate categoryParentCreate);
		public Task UpdateCategoryParentAsync(CategoryParentCreate categoryParentCreate);
		public Task DeleteCategoryParentAsync(int id);
	}
}
