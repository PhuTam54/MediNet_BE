using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IProductRepo
    {
        public Task<List<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        public Task<Product> AddProductAsync(ProductCreate productCreate);
        public Task UpdateProductAsync(ProductCreate productCreate);
        public Task DeleteProductAsync(int id);
    }
}
