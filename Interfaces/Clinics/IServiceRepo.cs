using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.DtoCreate.Orders.OrderServices;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface IServiceRepo
    {
        public Task<List<ServiceDto>> GetAllServiceAsync();
        public Task<ServiceDto> GetServiceByIdAsync(int id);
        public Task<Service> AddServiceAsync(ServiceCreate serviceCreate);
        public Task UpdateServiceAsync(ServiceCreate serviceCreate);
        public Task DeleteServiceAsync(int id);
    }
}
