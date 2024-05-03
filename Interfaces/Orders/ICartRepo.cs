using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface ICartRepo
	{
		public Task<List<CartReturnDto>> GetCartsByCustomerIdAsync(int userId);
		public Task<Cart> GetCartByIdAsync(int id);
		public Task<Cart> AddCartAsync(CartDto cartDto);
		public Task UpdateCartAsync(CartDto cartDto);
		public Task DeleteCartAsync(Cart cart);
		public Task<Cart> CheckCartExist(int productId, int clinicId, int customerId);
    }
}
