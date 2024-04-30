using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IOrderRepo
    {
        public Task<List<OrderDto>> GetAllOrderAsync();
        public Task<OrderDto> GetOrderByIdAsync(int id);
        public Task<Order> AddOrderAsync(OrderDto orderDto);
        public Task UpdateOrderAsync(OrderDto orderDto);
        public Task DeleteOrderAsync(int id);
    }
}
