using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface IProductRepo
	{
		public Task<List<Product>> GetAllProductAsync();
		public Task<Product> GetProductByIdAsync(int id);
		public Task<Product> AddProductAsync(ProductDto productDto);
		public Task UpdateProductAsync(ProductDto productDto);
		public Task DeleteProductAsync(Product product);
	}
}
