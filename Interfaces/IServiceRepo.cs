using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
    public interface IServiceRepo
	{
		public Task<List<Service>> GetAllServiceAsync();
		public Task<Service> GetServiceByIdAsync(int id);
		public Task<Service> AddServiceAsync(ServiceCreateDto serviceDto);
		public Task UpdateServiceAsync(ServiceCreateDto serviceDto);
		public Task DeleteServiceAsync(int id);
	}
}
