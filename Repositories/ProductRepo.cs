using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class ProductRepo : IProductRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;

		public ProductRepo(MediNetContext context, IMapper mapper, IFileService fileService)
		{
			_context = context;
			_mapper = mapper;
			_fileService = fileService;
		}

		public async Task<List<Product>> GetAllProductAsync()
		{
			var products = await _context.Products!.ToListAsync();
			return products;
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			var product = await _context.Products!.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
			return product;
		}

		public async Task<Product> AddProductAsync(ProductDto productDto)
		{
			var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productDto.CategoryChildId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == productDto.ClinicId);

			var productMap = _mapper.Map<Product>(productDto);
			productMap.CategoryChild = categoryChild;
			productMap.Clinic = clinic;

			_context.Products!.Add(productMap);
			await _context.SaveChangesAsync();
			return productMap;
		}

		public async Task UpdateProductAsync(ProductDto productDto)
		{
			var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productDto.CategoryChildId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == productDto.ClinicId);

			var productMap = _mapper.Map<Product>(productDto);
			productMap.CategoryChild = categoryChild;
			productMap.Clinic = clinic;


			_context.Products!.Update(productMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(Product product)
		{
			_context.Products!.Remove(product);
			await _context.SaveChangesAsync();
		}
	}
}
