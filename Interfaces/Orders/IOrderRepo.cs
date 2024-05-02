using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IOrderRepo
    {
        public Task<List<Order>> GetAllOrderAsync();
        public Task<Order> GetOrderByIdAsync(int id); 
        public Task<List<Order>> GetOrderByUserIdAsync(int userId);
        public Task<Order> AddOrderAsync(OrderDto orderDto);
        public Task UpdateOrderAsync(OrderDto orderDto);
        public Task DeleteOrderAsync(int id);
    }
}
