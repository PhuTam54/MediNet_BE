using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface ICategoryChildRepo
	{
		public Task<List<CategoryChild>> GetAllCategoryChildAsync();
		public Task<CategoryChild> GetCategoryChildByIdAsync(int id);
		public Task<CategoryChild> AddCategoryChildAsync(CategoryChildDto categoryChildDto);
		public Task UpdateCategoryChildAsync(CategoryChildDto categoryChildDto);
		public Task DeleteCategoryChildAsync(CategoryChild categoryChild);
	}
}
