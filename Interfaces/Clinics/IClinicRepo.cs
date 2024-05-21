using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface IClinicRepo
    {
        public Task<List<ClinicDto>> GetAllClinicAsync();
        public Task<ClinicDto> GetClinicByIdAsync(int id);
		public Task<List<ClinicDto>> GetClinicsByProductIdAsync(int productId);
		public Task<Clinic> AddClinicAsync(ClinicCreate clinicCreate);
        public Task UpdateClinicAsync(ClinicCreate clinicCreate);
        public Task DeleteClinicAsync(int id);
    }
}
