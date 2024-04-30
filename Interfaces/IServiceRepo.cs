using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface IServiceRepo
	{
		public Task<List<ServiceDto>> GetAllServiceAsync();
		public Task<ServiceDto> GetServiceByIdAsync(int id);
		public Task<Service> AddServiceAsync(ServiceDto serviceDto);
		public Task UpdateServiceAsync(ServiceDto serviceDto);
		public Task DeleteServiceAsync(int id);
	}
}
