using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Orders;
using MediNet_BE.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
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

        public async Task<List<ProductDto>> GetAllProductAsync()
        {
            var products = await _context.Products!
                .Include(cc => cc.CategoryChild)
                .Include(c => c.Clinic)
                .ToListAsync();
            var prdsMap = _mapper.Map<List<ProductDto>>(products);
            

            return prdsMap;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products!
				.Include(cc => cc.CategoryChild)
				.Include(c => c.Clinic)
				.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
			var prdMap = _mapper.Map<ProductDto>(product);

			return prdMap;
        }

        public async Task<Product> AddProductAsync(ProductCreateDto productDto)
        {
            var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productDto.CategoryChildId);
            var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == productDto.ClinicId);

            var productMap = _mapper.Map<Product>(productDto);
            productMap.Slug = CreateSlug.Init_Slug(productDto.Name);
            productMap.CategoryChild = categoryChild;
            productMap.Clinic = clinic;

            _context.Products!.Add(productMap);
            await _context.SaveChangesAsync();
            return productMap;
        }

        public async Task UpdateProductAsync(ProductCreateDto productDto)
        {
            var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == productDto.CategoryChildId);
            var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == productDto.ClinicId);

            var productMap = _mapper.Map<Product>(productDto);
            productMap.Slug = CreateSlug.Init_Slug(productDto.Name);
            productMap.CategoryChild = categoryChild;
            productMap.Clinic = clinic;


            _context.Products!.Update(productMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products!.SingleOrDefaultAsync(p => p.Id == id);
            _context.Products!.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
