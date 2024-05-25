using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees.Blogs
{
    public class DiseaseRepo : IDiseaseRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public DiseaseRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DiseaseDto>> GetAllDiseaseAsync()
        {
            var diseases = await _context.Diseases
                .Include(b => b.Blogs)
                .ToListAsync();

            var diseasesMap = _mapper.Map<List<DiseaseDto>>(diseases);

            return diseasesMap;
        }

        public async Task<DiseaseDto> GetDiseaseByIdAsync(int id)
        {
            var disease = await _context.Diseases
				.Include(b => b.Blogs)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var diseaseMap = _mapper.Map<DiseaseDto>(disease);

            return diseaseMap;
        }

        public async Task<Disease> AddDiseaseAsync(DiseaseCreate diseaseCreate)
        {
            var diseaseMap = _mapper.Map<Disease>(diseaseCreate);

            _context.Diseases!.Add(diseaseMap);
            await _context.SaveChangesAsync();
            return diseaseMap;
        }

        public async Task UpdateDiseaseAsync(DiseaseCreate diseaseCreate)
        {
            var diseaseMap = _mapper.Map<Disease>(diseaseCreate);

            _context.Diseases!.Update(diseaseMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiseaseAsync(int id)
        {
            var disease = await _context.Diseases!.FirstOrDefaultAsync(c => c.Id == id);
            _context.Diseases!.Remove(disease);
            await _context.SaveChangesAsync();
        }
    }
}
