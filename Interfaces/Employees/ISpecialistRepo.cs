using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Models.Employees;

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
