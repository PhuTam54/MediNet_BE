using AutoMapper;
using Humanizer.Localisation;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class OrderRepo : IOrderRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public OrderRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Order>> GetAllOrderAsync()
		{
			var orders = await _context.Orders!.ToListAsync();
			return orders;
		}

		public async Task<Order> GetOrderByIdAsync(int id)
		{
			var order = await _context.Orders!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return order;
		}

		public async Task<Order> AddOrderAsync(OrderDto orderDto)
		{
			var user = await _context.Customers!.FirstOrDefaultAsync(c => c.Id == orderDto.UserId);
			var orderMap = _mapper.Map<Order>(orderDto);
			orderMap.User = user;

			var carts = new List<Cart>();
			foreach (var carttId in orderDto.CartIds)
			{
				var cart = await _context.Carts.Include(p => p.Product).FirstOrDefaultAsync(c => c.Id == carttId);
				if (cart != null)
				{
					orderMap.TotalAmount += cart.SubTotal;
					carts.Add(cart);
				}
			}
			_context.Orders!.Add(orderMap);
			foreach (var item in carts)
			{
				var orderProduct = new OrderProduct { ProductId = item.Product.Id, OrderId = orderMap.Id, Product = item.Product, Order = orderMap, Quantity = item.QtyCart, Subtotal = item.SubTotal };
				var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == item.Product.Id);
				product.StockQuantity -= item.QtyCart;
				_context.OrderProducts!.Add(orderProduct);
				_context.Products!.Update(product);
				_context.Carts!.Remove(item);
			}
			

			await _context.SaveChangesAsync();
			return orderMap;
		}

		public async Task UpdateOrderAsync(OrderDto orderDto)
		{
			var orderMap = _mapper.Map<Order>(orderDto);
			_context.Orders!.Update(orderMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrderAsync(Order order)
		{
			_context.Orders!.Remove(order);
			await _context.SaveChangesAsync();
		}
	}
}
