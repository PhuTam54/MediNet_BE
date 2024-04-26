using MediNet_BE.Dto;
using MediNet_BE.Models;

namespace MediNet_BE.Interfaces
{
	public interface ICartRepo
	{
		public Task<List<Cart>> GetCartsByUserIdAsync(int userId);
		public Task<Cart> GetCartByIdAsync(int id);
		public Task<Cart> AddCartAsync(CartDto cartDto);
		public Task UpdateCartAsync(CartDto cartDto);
		public Task DeleteCartAsync(Cart cart);
	}
}
