using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Clinics;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Clinics
{
	public class StockInRepo : IStockInRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public StockInRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<StockInDto>> GetAllStockInAsync()
		{
			var stockIns = await _context.StockIns!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.ToListAsync();

			var stockInsMap = _mapper.Map<List<StockInDto>>(stockIns);

			return stockInsMap;
		}

		public async Task<StockInDto> GetStockInByIdAsync(int id)
		{
			var stockIn = await _context.StockIns!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var stockInMap = _mapper.Map<StockInDto>(stockIn);

			return stockInMap;
		}

		public async Task<StockInDto> GetStockInByProductIdAndClinicIdAsync(int productId, int clinicId)
		{
			var stockIn = await _context.StockIns!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Clinic.Id == clinicId);

			var stockInMap = _mapper.Map<StockInDto>(stockIn);

			return stockInMap;
		}

		public async Task<StockIn> AddStockInAsync(StockInCreate stockInCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == stockInCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == stockInCreate.ClinicId);
			var stockInMap = _mapper.Map<StockIn>(stockInCreate);
			stockInMap.Product = product;
			stockInMap.Clinic = clinic;

			_context.StockIns!.Add(stockInMap);
			await _context.SaveChangesAsync();
			return stockInMap;
		}

		public async Task UpdateStockInAsync(StockInCreate stockInCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == stockInCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == stockInCreate.ClinicId);
			var stockInMap = _mapper.Map<StockIn>(stockInCreate);
			stockInMap.Product = product;
			stockInMap.Clinic = clinic;

			_context.StockIns!.Update(stockInMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteStockInAsync(int id)
		{
			var stockIn = await _context.StockIns!.SingleOrDefaultAsync(c => c.Id == id);

			_context.StockIns!.Remove(stockIn);
			await _context.SaveChangesAsync();
		}
	}
}
