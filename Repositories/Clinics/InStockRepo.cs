using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Clinics;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Clinics
{
    public class InStockRepo : IInStockRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public InStockRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<InStockDto>> GetAllInStockAsync()
		{
			var inStocks = await _context.InStocks!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.ToListAsync();

			var inStocksMap = _mapper.Map<List<InStockDto>>(inStocks);

			return inStocksMap;
		}

		public async Task<InStockDto> GetInStockByIdAsync(int id)
		{
			var inStock = await _context.InStocks!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var inStockMap = _mapper.Map<InStockDto>(inStock);

			return inStockMap;
		}

		public async Task<InStockDto> GetInStockByProductIdAndClinicIdAsync(int productId, int clinicId)
		{
			var inStock = await _context.InStocks!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Clinic.Id == clinicId);

			var inStockMap = _mapper.Map<InStockDto>(inStock);

			return inStockMap;
		}

		public async Task<InStock> AddInStockAsync(InStockCreate inStockCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == inStockCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == inStockCreate.ClinicId);
			var inStockMap = _mapper.Map<InStock>(inStockCreate);
			inStockMap.Product = product;
			inStockMap.Clinic = clinic;
			inStockMap.LastUpdatedAt = DateTime.UtcNow;

			_context.InStocks!.Add(inStockMap);
			await _context.SaveChangesAsync();
			return inStockMap;
		}

		public async Task UpdateInStockAsync(InStockCreate inStockCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == inStockCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == inStockCreate.ClinicId);
			var inStockMap = _mapper.Map<InStock>(inStockCreate);
			inStockMap.Product = product;
			inStockMap.Clinic = clinic;
			inStockMap.LastUpdatedAt = DateTime.UtcNow;

			_context.InStocks!.Update(inStockMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteInStockAsync(int id)
		{
			var inStock = await _context.InStocks!.SingleOrDefaultAsync(c => c.Id == id);

			_context.InStocks!.Remove(inStock);
			await _context.SaveChangesAsync();
		}


	}
}
