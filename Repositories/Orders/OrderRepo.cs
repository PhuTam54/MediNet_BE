using AutoMapper;
using Humanizer.Localisation;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Payments.VNPay;
using MediNet_BE.DtoCreate.Orders;
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

        public OrderRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<OrderDto>> GetAllOrderAsync()
        {
            var orders = await _context.Orders!
                .Include(c => c.Customer)
                .Include(op => op.OrderProducts)
                .ThenInclude(p => p.Product)
                .Include(os => os.OrderServices)
                .ThenInclude(s => s.Service)
                .ToListAsync();

            var ordersMap = _mapper.Map<List<OrderDto>>(orders);

            return ordersMap;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders!
				.Include(c => c.Customer)
				.Include(op => op.OrderProducts)
				.ThenInclude(p => p.Product)
				.Include(os => os.OrderServices)
				.ThenInclude(s => s.Service)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var orderMap = _mapper.Map<OrderDto>(order);

            return orderMap;
        }

        public async Task<List<OrderDto>> GetOrderByUserIdAsync(int userId)
        {
            var orders = await _context.Orders!
				.Include(c => c.Customer)
				.Include(op => op.OrderProducts)
				.ThenInclude(p => p.Product)
				.Include(os => os.OrderServices)
				.ThenInclude(s => s.Service)
				.AsNoTracking()
                .Where(c => c.Customer.Id == userId)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
            var ordersMap = _mapper.Map<List<OrderDto>>(orders);

            return ordersMap;
        }

        public async Task<Order> AddOrderAsync(OrderCreate orderCreate)
        {
			var random = new Random().Next(1000, 10000);

			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == orderCreate.CustomerId);
            var orderMap = _mapper.Map<Order>(orderCreate);
            orderMap.OrderCode = (DateTime.UtcNow.Ticks + random).ToString();
			orderMap.Status = OrderStatus.PENDING;
			orderMap.Is_paid = false;
			orderMap.OrderDate = DateTime.UtcNow;
            orderMap.Customer = customer;

			_context.Orders.Add(orderMap);
			foreach (var carttId in orderCreate.CartIds)
			{
				var cart = await _context.Carts.Include(p => p.Product).FirstOrDefaultAsync(c => c.Id == carttId);
				if (cart != null)
				{
					var orderProduct = new OrderProduct { ProductId = cart.Product.Id, OrderId = orderMap.Id, Product = cart.Product, Order = orderMap, Quantity = cart.QtyCart, Subtotal = cart.SubTotal };
					var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Product.Id == cart.ProductId && s.Clinic.Id == cart.ClinicId);
					if (supply != null)
						{
							supply.StockQuantity -= cart.QtyCart;
							_context.Supplies.Update(supply);
						}
					_context.OrderProducts.Add(orderProduct);
					_context.Carts.Remove(cart);
				}
			}
			
			await _context.SaveChangesAsync();
			return orderMap;
		}

		public async Task UpdateOrderAsync(int id, OrderStatus orderStatus)
		{
			var order = await _context.Orders!.FirstOrDefaultAsync(o => o.Id == id);
			if(order != null)
			{
				order.Status = orderStatus;
				_context.Orders!.Update(order);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteOrderAsync(int id)
        {
			var order = await _context.Orders!.FirstOrDefaultAsync(c => c.Id == id);

			_context.Orders!.Remove(order);
            await _context.SaveChangesAsync();
        }

		
	}
}
