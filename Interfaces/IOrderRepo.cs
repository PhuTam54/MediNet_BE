using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface IOrderRepo
	{
		public Task<List<Order>> GetAllOrderAsync();
		public Task<Order> GetOrderByIdAsync(int id);
		public Task<Order> AddOrderAsync(OrderDto orderDto);
		public Task UpdateOrderAsync(OrderDto orderDto);
		public Task DeleteOrderAsync(Order order);
	}
}
