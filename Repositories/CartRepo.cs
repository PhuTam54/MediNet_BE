using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class CartRepo : ICartRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CartRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Cart>> GetCartsByUserIdAsync(int userId)
		{
			var carts = await _context.Carts.Where(c => c.User.Id == userId).ToListAsync();
			return carts;
		}

		public async Task<Cart> GetCartByIdAsync(int id)
		{
			var cart = await _context.Carts!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return cart;
		}

		public async Task<Cart> AddCartAsync(CartDto cartDto)
		{
			var user = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartDto.UserID);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartDto.ProductID);

			var cartMap = _mapper.Map<Cart>(cartDto);
			cartMap.User = user;
			cartMap.Product = product;
			cartMap.SubTotal = Math.Round(product.Price * cartDto.QtyCart, 2);

			_context.Carts!.Add(cartMap);
			await _context.SaveChangesAsync();
			return cartMap;
		}

		public async Task UpdateCartAsync(CartDto cartDto)
		{
			var user = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartDto.UserID);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartDto.ProductID);

			var cartMap = _mapper.Map<Cart>(cartDto);
			cartMap.User = user;
			cartMap.Product = product;
			cartMap.SubTotal = Math.Round(product.Price * cartDto.QtyCart, 2);

			_context.Carts!.Update(cartMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCartAsync(Cart cart)
		{
			_context.Carts!.Remove(cart);
			await _context.SaveChangesAsync();
		}

		
	}
}
