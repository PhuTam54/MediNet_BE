using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
    public interface IClinicRepo
	{
		public Task<List<Clinic>> GetAllClinicAsync();
		public Task<Clinic> GetClinicByIdAsync(int id);
		public Task<Clinic> AddClinicAsync(ClinicDto clinicDto);
		public Task UpdateClinicAsync(ClinicDto clinicDto);
		public Task DeleteClinicAsync(Clinic clinic);
	}
}
