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
    public class ClinicRepo : IClinicRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public ClinicRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClinicDto>> GetAllClinicAsync()
        {
            var clinics = await _context.Clinics
                .Include(e => e.Employees)
                .ToListAsync();

            var clinicsMap = _mapper.Map<List<ClinicDto>>(clinics);

            return clinicsMap;
        }

        public async Task<ClinicDto> GetClinicByIdAsync(int id)
        {
            var clinic = await _context.Clinics
				.Include(e => e.Employees)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

			var clinicMap = _mapper.Map<ClinicDto>(clinic);

			return clinicMap;
        }

        public async Task<Clinic> AddClinicAsync(ClinicCreate clinicCreate)
        {
            var clinicMap = _mapper.Map<Clinic>(clinicCreate);
            clinicMap.Slug = CreateSlug.Init_Slug(clinicCreate.Name);

            _context.Clinics!.Add(clinicMap);
            await _context.SaveChangesAsync();
            return clinicMap;
        }

        public async Task UpdateClinicAsync(ClinicCreate clinicCreate)
        {
            var clinicMap = _mapper.Map<Clinic>(clinicCreate);
            clinicMap.Slug = CreateSlug.Init_Slug(clinicCreate.Name);

            _context.Clinics!.Update(clinicMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClinicAsync(int id)
        {
            var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == id);
            _context.Clinics!.Remove(clinic);
            await _context.SaveChangesAsync();
        }

    }
}
