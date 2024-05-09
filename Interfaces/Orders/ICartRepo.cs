using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface ICartRepo
	{
		public Task<List<CartDto>> GetCartsByCustomerIdAsync(int userId);
		public Task<CartDto> GetCartByIdAsync(int id);
		public Task<Cart> AddCartAsync(CartCreate cartCreate);
		public Task UpdateCartAsync(CartCreate cartCreate);
		public Task DeleteCartAsync(int id);
		public Task<Cart> CheckCartExist(int productId, int clinicId, int customerId);
    }
}
