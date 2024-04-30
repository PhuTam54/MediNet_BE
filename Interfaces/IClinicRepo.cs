using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
    public interface IClinicRepo
	{
		public Task<List<ClinicDto>> GetAllClinicAsync();
		public Task<ClinicDto> GetClinicByIdAsync(int id);
		public Task<Clinic> AddClinicAsync(ClinicDto clinicDto);
		public Task UpdateClinicAsync(ClinicDto clinicDto);
		public Task DeleteClinicAsync(int id);
	}
}
