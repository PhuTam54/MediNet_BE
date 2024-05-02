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
            var clinics = await _context.Clinics!
                .Include(s => s.Services)
                .Include(s => s.Supplies)
                .ToListAsync();

            return clinics;
        }

        public async Task<Clinic> GetClinicByIdAsync(int id)
        {
            var clinic = await _context.Clinics!
                .Include(s => s.Services)
				.Include(s => s.Supplies)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return clinic;
        }

        public async Task<Clinic> AddClinicAsync(ClinicDto clinicDto)
        {
            var clinicMap = _mapper.Map<Clinic>(clinicDto);
            clinicMap.Slug = CreateSlug.Init_Slug(clinicDto.Name);

            _context.Clinics!.Add(clinicMap);
            await _context.SaveChangesAsync();
            return clinicMap;
        }

        public async Task UpdateClinicAsync(ClinicDto clinicDto)
        {
            var clinicMap = _mapper.Map<Clinic>(clinicDto);
            clinicMap.Slug = CreateSlug.Init_Slug(clinicDto.Name);

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
