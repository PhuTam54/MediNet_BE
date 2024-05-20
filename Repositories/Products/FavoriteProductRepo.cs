using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Products;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Interfaces.Products;
using MediNet_BE.Models.Products;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Products
{
	public class FavoriteProductRepo : IFavoriteProductRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public FavoriteProductRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<FavoriteProductDto>> GetAllFavoriteProductAsync()
		{
			var favoriteProducts = await _context.FavoriteProducts!
				.Include(p => p.Product)
				.Include(c => c.Customer)
				.ToListAsync();

			var favoriteProductsMap = _mapper.Map<List<FavoriteProductDto>>(favoriteProducts);

			return favoriteProductsMap;
		}

		public async Task<FavoriteProductDto> GetFavoriteProductByIdAsync(int id)
		{
			var favoriteProduct = await _context.FavoriteProducts!
				.Include(p => p.Product)
				.Include(c => c.Customer)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var favoriteProductMap = _mapper.Map<FavoriteProductDto>(favoriteProduct);

			return favoriteProductMap;
		}

		public async Task<List<FavoriteProductDto>> GetFavoriteProductsByCustomerIdAsync(int customerId)
		{
			var favoriteProducts = await _context.FavoriteProducts!
				.Include(p => p.Product)
				.Include(c => c.Customer)
				.Where(fp => fp.Customer.Id == customerId)
				.ToListAsync();

			var favoriteProductsMap = _mapper.Map<List<FavoriteProductDto>>(favoriteProducts);

			return favoriteProductsMap;
		}

		public async Task<List<FavoriteProductDto>> GetFavoriteProductsByProductIdAsync(int productId)
		{
			var favoriteProducts = await _context.FavoriteProducts!
				.Include(p => p.Product)
			    .Include(c => c.Customer)
				.Where(fp => fp.Product.Id == productId)
				.ToListAsync();

			var favoriteProductsMap = _mapper.Map<List<FavoriteProductDto>>(favoriteProducts);

			return favoriteProductsMap;
		}

		public async Task<FavoriteProduct> AddFavoriteProductAsync(FavoriteProductCreate favoriteProductCreate)
		{
			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == favoriteProductCreate.CustomerId);
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == favoriteProductCreate.ProductId);

			var favoriteProductMap = _mapper.Map<FavoriteProduct>(favoriteProductCreate);
			favoriteProductMap.Customer = customer;
			favoriteProductMap.Product = product;
			favoriteProductMap.CreatedAt = DateTime.UtcNow;

			_context.FavoriteProducts!.Add(favoriteProductMap);
			await _context.SaveChangesAsync();
			return favoriteProductMap;
		}

		public async Task DeleteFavoriteProductAsync(int id)
		{
			var favoriteProduct = await _context.FavoriteProducts!.FirstOrDefaultAsync(c => c.Id == id);
			_context.FavoriteProducts!.Remove(favoriteProduct);
			await _context.SaveChangesAsync();
		}

	}
}
