using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Clinics
{
	public class SupplyRepo : ISupplyRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public SupplyRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Supply>> GetAllSupplyAsync()
		{
			var supplies = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.ToListAsync();
			return supplies;
		}

		public async Task<Supply> GetSupplyByIdAsync(int id)
		{
			var supply = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return supply;
		}

		public async Task<Supply> GetSupplyByProductIdAndClinicIdAsync(int productId, int clinicId)
		{
			var supply = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Clinic.Id == clinicId);

			return supply;
		}

		public async Task<Supply> AddSupplyAsync(SupplyDto supplyDto)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == supplyDto.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == supplyDto.ClinicId);
			var supplyMap = _mapper.Map<Supply>(supplyDto);
			supplyMap.Product = product;
			supplyMap.Clinic = clinic;

			_context.Supplies!.Add(supplyMap);
			await _context.SaveChangesAsync();
			return supplyMap;
		}

		public async Task UpdateSupplyAsync(SupplyDto supplyDto)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == supplyDto.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == supplyDto.ClinicId);
			var supplyMap = _mapper.Map<Supply>(supplyDto);
			supplyMap.Product = product;
			supplyMap.Clinic = clinic;

			_context.Supplies!.Update(supplyMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteSupplyAsync(int id)
		{
			var supply = await _context.Supplies!.SingleOrDefaultAsync(c => c.Id == id);

			_context.Supplies!.Remove(supply);
			await _context.SaveChangesAsync();
		}

		
	}
}
