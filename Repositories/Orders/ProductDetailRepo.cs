using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
{
    public class ProductDetailRepo : IProductDetailRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public ProductDetailRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<ProductDetailDto>> GetAllProductDetailAsync()
		{
			var productDetails = await _context.ProductDetails!
				.ToListAsync();

			var productDetailsMap = _mapper.Map<List<ProductDetailDto>>(productDetails);

			return productDetailsMap;
		}

		public async Task<ProductDetailDto> GetProductDetailByIdAsync(int id)
		{
			var productDetail = await _context.ProductDetails!
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var productDetailMap = _mapper.Map<ProductDetailDto>(productDetail);

			return productDetailMap;
		}

		public async Task<ProductDetail> AddProductDetailAsync(ProductDetailCreate productDetailCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == productDetailCreate.ProductId);
			var productDetailMap = _mapper.Map<ProductDetail>(productDetailCreate);
			productDetailMap.Product = product;

			_context.ProductDetails!.Add(productDetailMap);
			await _context.SaveChangesAsync();
			return productDetailMap;
		}

		public async Task UpdateProductDetailAsync(ProductDetailCreate productDetailCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == productDetailCreate.ProductId);
			var productDetailMap = _mapper.Map<ProductDetail>(productDetailCreate);
			productDetailMap.Product = product;

			_context.ProductDetails!.Update(productDetailMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductDetailAsync(int id)
		{
			var productDetail = await _context.ProductDetails!.FirstOrDefaultAsync(c => c.Id == id);
			_context.ProductDetails!.Remove(productDetail);
			await _context.SaveChangesAsync();
		}
	}
}
