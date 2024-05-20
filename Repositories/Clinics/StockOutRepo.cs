using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Clinics;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Clinics
{
	public class StockOutRepo : IStockOutRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public StockOutRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<StockOutDto>> GetAllStockOutAsync()
		{
			var stockOuts = await _context.StockOuts!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.ToListAsync();

			var stockOutsMap = _mapper.Map<List<StockOutDto>>(stockOuts);

			return stockOutsMap;
		}

		public async Task<StockOutDto> GetStockOutByIdAsync(int id)
		{
			var stockOut = await _context.StockOuts!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var stockOutMap = _mapper.Map<StockOutDto>(stockOut);

			return stockOutMap;
		}

		public async Task<StockOutDto> GetStockOutByProductIdAndClinicIdAsync(int productId, int clinicId)
		{
			var stockOut = await _context.StockOuts!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Clinic.Id == clinicId);

			var stockOutMap = _mapper.Map<StockOutDto>(stockOut);

			return stockOutMap;
		}

		public async Task<StockOut> AddStockOutAsync(StockOutCreate stockOutCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == stockOutCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == stockOutCreate.ClinicId);
			var stockOutMap = _mapper.Map<StockOut>(stockOutCreate);
			stockOutMap.Product = product;
			stockOutMap.Clinic = clinic;

			_context.StockOuts!.Add(stockOutMap);
			await _context.SaveChangesAsync();
			return stockOutMap;
		}

		public async Task UpdateStockOutAsync(StockOutCreate stockOutCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == stockOutCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == stockOutCreate.ClinicId);
			var stockOutMap = _mapper.Map<StockOut>(stockOutCreate);
			stockOutMap.Product = product;
			stockOutMap.Clinic = clinic;

			_context.StockOuts!.Update(stockOutMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteStockOutAsync(int id)
		{
			var stockOut = await _context.StockOuts!.SingleOrDefaultAsync(c => c.Id == id);

			_context.StockOuts!.Remove(stockOut);
			await _context.SaveChangesAsync();
		}
	}
}
