using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface ICategoryRepo
	{
		public Task<List<Category>> GetAllCategoryAsync();
		public Task<Category> GetCategoryByIdAsync(int id);
		public Task<Category> AddCategoryAsync(CategoryDto categoryDto);
		public Task UpdateCategoryAsync(CategoryDto categoryDto);
		public Task DeleteCategoryAsync(Category category);
	}
}
