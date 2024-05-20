using MediNet_BE.Dto.Products;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Models.Products;

namespace MediNet_BE.Interfaces.Products
{
	public interface IFavoriteProductRepo
	{
		public Task<List<FavoriteProductDto>> GetAllFavoriteProductAsync(); 
		public Task<FavoriteProductDto> GetFavoriteProductByIdAsync(int id);
		public Task<List<FavoriteProductDto>> GetFavoriteProductsByCustomerIdAsync(int customerId);
		public Task<List<FavoriteProductDto>> GetFavoriteProductsByProductIdAsync(int productId);
		public Task<FavoriteProduct> AddFavoriteProductAsync(FavoriteProductCreate favoriteProductCreate);
		public Task DeleteFavoriteProductAsync(int id);
	}
}
