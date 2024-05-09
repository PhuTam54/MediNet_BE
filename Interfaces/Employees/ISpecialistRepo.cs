using MediNet_BE.Dto.Doctors;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Models.Doctors;

namespace MediNet_BE.Interfaces.Employees
{
    public interface ISpecialistRepo
    {
        public Task<List<SpecialistDto>> GetAllSpecialistAsync();
        public Task<SpecialistDto> GetSpecialistByIdAsync(int id);
        public Task<Specialist> AddSpecialistAsync(SpecialistCreate specialistCreate);
        public Task UpdateSpecialistAsync(SpecialistCreate specialistCreate);
        public Task DeleteSpecialistAsync(int id);
    }
}
