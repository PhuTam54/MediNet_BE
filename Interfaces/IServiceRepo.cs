using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface IServiceRepo
	{
		public Task<List<Service>> GetAllServiceAsync();
		public Task<Service> GetServiceByIdAsync(int id);
		public Task<Service> AddServiceAsync(ServiceDto serviceDto);
		public Task UpdateServiceAsync(ServiceDto serviceDto);
		public Task DeleteServiceAsync(Service service);
	}
}
