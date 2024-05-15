using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Orders;
using MediNet_BE.Services.Image;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MediNet_BE.Repositories.Orders
{
    public class ProductRepo : IProductRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public ProductRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllProductAsync()
        {
            var products = await _context.Products!
				.Include(cc => cc.CategoryChild)
                .ThenInclude(c => c.Category)
                .ThenInclude(cp => cp.CategoryParent)
                .Include(op => op.OrderProducts)
                .Include(c => c.Carts)
                .Include(s => s.Supplies)
                .ToListAsync();

            var prdsMap = _mapper.Map<List<ProductDto>>(products);

            return prdsMap;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products!
				.Include(cc => cc.CategoryChild)
				.Include(op => op.OrderProducts)
				.Include(c => c.Carts)
				.Include(s => s.Supplies)
				.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

			var prdMap = _mapper.Map<ProductDto>(product);

			return prdMap;
        }

		public async Task<List<ProductDto>> GetProductsByCategoryChildIdAsync(int categoryChildId)
		{
            var products = await _context.Products!
					.Where(p => p.CategoryChild.Id == categoryChildId)
					.AsNoTracking()
	                .ToListAsync();

			var prdsMap = _mapper.Map<List<ProductDto>>(products);

			return prdsMap;
		}

		public async Task<List<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
		{
			var products = await _context.Products!
					.Where(p => p.CategoryChild.Category.Id == categoryId)
					.AsNoTracking()
					.ToListAsync();

			var prdsMap = _mapper.Map<List<ProductDto>>(products);

			return prdsMap;
		}

		public async Task<List<ProductDto>> GetProductsByCategoryParentIdAsync(int categoryParentId)
		{
			var products = await _context.Products!
					.Where(p => p.CategoryChild.Category.CategoryParent.Id == categoryParentId)
					.AsNoTracking()
					.ToListAsync();

			var prdsMap = _mapper.Map<List<ProductDto>>(products);

			return prdsMap;
		}

		public async Task<Product> AddProductAsync(ProductCreate productCreate)
        {
            var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productCreate.CategoryChildId);

            var productMap = _mapper.Map<Product>(productCreate);
            productMap.Slug = CreateSlug.Init_Slug(productCreate.Name);
            productMap.CategoryChild = categoryChild;

            _context.Products!.Add(productMap);
            await _context.SaveChangesAsync();

            return productMap;
        }

        public async Task UpdateProductAsync(ProductCreate productCreate)
        {
            var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productCreate.CategoryChildId);

            var productMap = _mapper.Map<Product>(productCreate);
            productMap.Slug = CreateSlug.Init_Slug(productCreate.Name);
            productMap.CategoryChild = categoryChild;

            _context.Products!.Update(productMap);
            await _context.SaveChangesAsync();
		}

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products!.SingleOrDefaultAsync(p => p.Id == id);
            _context.Products!.Remove(product);
            await _context.SaveChangesAsync();
        }

		public async Task<List<ProductDto>> GetProductsByBuyQtyAsync()
		{
			var products = await _context.Products!
				.Include(cc => cc.CategoryChild)
				.ThenInclude(c => c.Category)
				.ThenInclude(cp => cp.CategoryParent)
				.Include(op => op.OrderProducts)
				.Include(c => c.Carts)
				.Include(s => s.Supplies)
				.Select(p => new
				{
					Product = p,
					BuyQty = p.OrderProducts.Sum(op => op.Quantity)
				})
		        .OrderByDescending(p => p.BuyQty)
				.ToListAsync();

			var prdsMap = _mapper.Map<List<ProductDto>>(products.Select(c => c.Product).ToList());

			return prdsMap;
		}

	}
}
