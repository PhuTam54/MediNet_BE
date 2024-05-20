using MediNet_BE.Dto.Products;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Models.Products;

namespace MediNet_BE.Interfaces.Products
{
    public interface IProductDetailRepo
    {
        public Task<List<ProductDetailDto>> GetAllProductDetailAsync();
        public Task<ProductDetailDto> GetProductDetailByIdAsync(int id);
        public Task<List<ProductDetailDto>> GetProductDetailsByProductIdAsync(int productId);
        public Task<ProductDetail> AddProductDetailAsync(ProductDetailCreate productDetailCreate);
        public Task UpdateProductDetailAsync(ProductDetailCreate productDetailCreate);
        public Task DeleteProductDetailAsync(int id);
    }
}
