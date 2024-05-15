using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees
{
    public class SpecialistRepo : ISpecialistRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public SpecialistRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SpecialistDto>> GetAllSpecialistAsync()
        {
            var specialists = await _context.Specialists!
                .ToListAsync();

            var specialistsMap = _mapper.Map<List<SpecialistDto>>(specialists);

            return specialistsMap;
        }

        public async Task<SpecialistDto> GetSpecialistByIdAsync(int id)
        {
            var specialist = await _context.Specialists!
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var specialistMap = _mapper.Map<SpecialistDto>(specialist);

            return specialistMap;
        }

        public async Task<Specialist> AddSpecialistAsync(SpecialistCreate specialistCreate)
        {
            var specialistMap = _mapper.Map<Specialist>(specialistCreate);

            _context.Specialists!.Add(specialistMap);
            await _context.SaveChangesAsync();
            return specialistMap;
        }

        public async Task UpdateSpecialistAsync(SpecialistCreate specialistCreate)
        {
            var specialistMap = _mapper.Map<Specialist>(specialistCreate);

            _context.Specialists!.Update(specialistMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecialistAsync(int id)
        {
            var specialist = await _context.Specialists!.FirstOrDefaultAsync(c => c.Id == id);
            _context.Specialists!.Remove(specialist);
            await _context.SaveChangesAsync();
        }
    }
}
