using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IProductDetailRepo
	{
		public Task<List<ProductDetailDto>> GetAllProductDetailAsync();
		public Task<ProductDetailDto> GetProductDetailByIdAsync(int id);
		public Task<ProductDetail> AddProductDetailAsync(ProductDetailCreate productDetailCreate);
		public Task UpdateProductDetailAsync(ProductDetailCreate productDetailCreate);
		public Task DeleteProductDetailAsync(int id);
	}
}
