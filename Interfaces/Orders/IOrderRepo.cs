using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IOrderRepo
    {
        public Task<List<OrderReturnDto>> GetAllOrderAsync();
        public Task<OrderReturnDto> GetOrderByIdAsync(int id); 
        public Task<List<OrderReturnDto>> GetOrderByUserIdAsync(int userId);
        public Task<Order> AddOrderAsync(OrderDto orderDto);
        public Task UpdateOrderAsync(OrderDto orderDto);
        public Task DeleteOrderAsync(int id);
    }
}
