using AutoMapper;
using Humanizer.Localisation;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Payments.VNPay;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Orders;
using MediNet_BE.Services;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
{
    public class OrderRepo : IOrderRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public OrderRepo(MediNetContext context, IMapper mapper, IMailService mailService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
        }


        public async Task<List<Order>> GetAllOrderAsync()
        {
            var orders = await _context.Orders!
                .Include(c => c.Customer)
                .Include(op => op.OrderProducts)
                .Include(os => os.OrderServices)
                .ToListAsync();

			return orders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders!
				.Include(c => c.Customer)
				.Include(op => op.OrderProducts)
				.Include(os => os.OrderServices)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

			return order;
        }

        public async Task<List<Order>> GetOrderByUserIdAsync(int userId)
        {
            var order = await _context.Orders!
                .Include(c => c.Customer)
                .Include(op => op.OrderProducts)
                .Include(os => os.OrderServices)
                .Where(c => c.Customer.Id == userId)
                .ToListAsync();
            var orderMap = _mapper.Map<List<Order>>(order);

            return orderMap;
        }

        public async Task<Order> AddOrderAsync(OrderDto orderDto)
        {
			var random = new Random().Next(1000, 10000);

			var customer = await _context.Customers!.FirstOrDefaultAsync(c => c.Id == orderDto.CustomerId);
            var orderMap = _mapper.Map<Order>(orderDto);
            orderMap.OrderCode = (DateTime.UtcNow.Ticks + random).ToString();
			orderMap.Status = OrderStatus.PENDING;
            orderMap.Customer = customer;

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
                var supply = await _context.Supplies!.FirstOrDefaultAsync(s => s.Product.Id == item.ProductId && s.Clinic.Id == item.ClinicId);
                if(supply != null)
                {
					supply.StockQuantity -= item.QtyCart;
				}
				_context.OrderProducts!.Add(orderProduct);
				_context.Supplies!.Update(supply);
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

        public async Task DeleteOrderAsync(int id)
        {
			var order = await _context.Orders!.FirstOrDefaultAsync(c => c.Id == id);

			_context.Orders!.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
