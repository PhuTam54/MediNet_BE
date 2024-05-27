using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders;
using MediNet_BE.DtoCreate.Orders;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models.Orders;
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
				.OrderByDescending(o => o.Id)
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
				.OrderByDescending(o => o.Id)
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
                .OrderByDescending(o => o.Id)
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
					var orderProduct = new OrderProduct { ProductId = cart.Product.Id, OrderId = orderMap.Id, Product = cart.Product, Order = orderMap, Quantity = cart.QtyCart, PriceSale = cart.Product.Price };
					var inStock = await _context.InStocks.FirstOrDefaultAsync(s => s.Product.Id == cart.ProductId && s.Clinic.Id == cart.ClinicId);
					if (inStock != null)
						{
						    inStock.StockQuantity -= cart.QtyCart;
						    inStock.QuantitySold += cart.QtyCart;
							_context.InStocks.Update(inStock);
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
