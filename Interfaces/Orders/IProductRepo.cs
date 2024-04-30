using MediNet_BE.Dto.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IProductRepo
    {
        public Task<List<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        public Task<Product> AddProductAsync(ProductDto productDto);
        public Task UpdateProductAsync(ProductDto productDto);
        public Task DeleteProductAsync(int id);
    }
}
