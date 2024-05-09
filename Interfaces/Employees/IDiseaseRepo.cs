using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Doctors;

namespace MediNet_BE.Interfaces.Employees
{
    public interface IDiseaseRepo
    {
        public Task<List<DiseaseDto>> GetAllDiseaseAsync();
        public Task<DiseaseDto> GetDiseaseByIdAsync(int id);
        public Task<Disease> AddDiseaseAsync(DiseaseCreate diseaseCreate);
        public Task UpdateDiseaseAsync(DiseaseCreate diseaseCreate);
        public Task DeleteDiseaseAsync(int id);
    }
}
