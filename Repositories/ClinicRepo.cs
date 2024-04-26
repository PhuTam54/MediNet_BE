using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
    public class ClinicRepo : IClinicRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public ClinicRepo(MediNetContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Clinic>> GetAllClinicAsync()
		{
			var clinics = await _context.Clinics!.ToListAsync();
			return clinics;
		}

		public async Task<Clinic> GetClinicByIdAsync(int id)
		{
			var clinic = await _context.Clinics!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return clinic;
		}

		public async Task<Clinic> AddClinicAsync(ClinicDto clinicDto)
		{
			var clinicMap = _mapper.Map<Clinic>(clinicDto);
			_context.Clinics!.Add(clinicMap);
			await _context.SaveChangesAsync();
			return clinicMap;
		}

		public async Task UpdateClinicAsync(ClinicDto clinicDto)
		{
			var clinicMap = _mapper.Map<Clinic>(clinicDto);
			_context.Clinics!.Update(clinicMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteClinicAsync(Clinic clinic)
		{
			_context.Clinics!.Remove(clinic);
			await _context.SaveChangesAsync();
		}

	}
}
