using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface IServiceRepo
	{
		public Task<List<ServiceDto>> GetAllServiceAsync();
		public Task<ServiceDto> GetServiceByIdAsync(int id);
		public Task<Service> AddServiceAsync(ServiceCreateDto serviceDto);
		public Task UpdateServiceAsync(ServiceCreateDto serviceDto);
		public Task DeleteServiceAsync(int id);
	}
}
