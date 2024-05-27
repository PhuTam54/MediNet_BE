using MediNet_BE.Dto.Orders;
using MediNet_BE.DtoCreate.Orders;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IOrderRepo
    {
        public Task<List<OrderDto>> GetAllOrderAsync();
        public Task<OrderDto> GetOrderByIdAsync(int id); 
        public Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        public Task<Order> AddOrderAsync(OrderCreate orderCreate);
        public Task UpdateOrderAsync(int id, OrderStatus orderStatus);
        public Task DeleteOrderAsync(int id);
    }
}
