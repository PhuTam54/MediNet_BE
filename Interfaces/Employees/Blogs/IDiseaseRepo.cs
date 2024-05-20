using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;

namespace MediNet_BE.Interfaces.Employees.Blogs
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
