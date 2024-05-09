using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
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

		public async Task<List<SupplyDto>> GetAllSupplyAsync()
		{
			var supplies = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.ToListAsync();

			var suppliesMap = _mapper.Map<List<SupplyDto>>(supplies);

			return suppliesMap;
		}

		public async Task<SupplyDto> GetSupplyByIdAsync(int id)
		{
			var supply = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var supplyMap = _mapper.Map<SupplyDto>(supply);

			return supplyMap;
		}

		public async Task<SupplyDto> GetSupplyByProductIdAndClinicIdAsync(int productId, int clinicId)
		{
			var supply = await _context.Supplies!
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Clinic.Id == clinicId);

			var supplyMap = _mapper.Map<SupplyDto>(supply);

			return supplyMap;
		}

		public async Task<Supply> AddSupplyAsync(SupplyCreate supplyCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == supplyCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == supplyCreate.ClinicId);
			var supplyMap = _mapper.Map<Supply>(supplyCreate);
			supplyMap.Product = product;
			supplyMap.Clinic = clinic;

			_context.Supplies!.Add(supplyMap);
			await _context.SaveChangesAsync();
			return supplyMap;
		}

		public async Task UpdateSupplyAsync(SupplyCreate supplyCreate)
		{
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == supplyCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == supplyCreate.ClinicId);
			var supplyMap = _mapper.Map<Supply>(supplyCreate);
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
