using MediNet_BE.Dto.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface IClinicRepo
    {
        public Task<List<Clinic>> GetAllClinicAsync();
        public Task<Clinic> GetClinicByIdAsync(int id);
        public Task<Clinic> AddClinicAsync(ClinicDto clinicDto);
        public Task UpdateClinicAsync(ClinicDto clinicDto);
        public Task DeleteClinicAsync(int id);
    }
}
