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


        public async Task<List<OrderDto>> GetAllOrderAsync()
        {
            var orders = await _context.Orders!
                .Include(c => c.Customer)
                .Include(op => op.OrderProducts)
                .Include(os => os.OrderServices)
                .ToListAsync();
			var ordersMap = _mapper.Map<List<OrderDto>>(orders);

			return ordersMap;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders!
				.Include(c => c.Customer)
				.Include(op => op.OrderProducts)
				.Include(os => os.OrderServices)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
			var orderMap = _mapper.Map<OrderDto>(order);

			return orderMap;
        }

        public async Task<Order> AddOrderAsync(OrderCreateDto orderDto)
        {
			var random = new Random().Next(1000, 10000);

			var customer = await _context.Customers!.FirstOrDefaultAsync(c => c.Id == orderDto.CustomerId);
            var orderMap = _mapper.Map<Order>(orderDto);
            orderMap.OrderCode = (DateTime.UtcNow.Ticks + random).ToString();
			orderMap.Status = OrderStatus.PENDING;
            orderMap.Customer = customer;

            _context.Orders!.Add(orderMap);
            await _context.SaveChangesAsync();

            var orderProducts = new List<OrderProduct>();
            foreach (CartItem cartItem in orderDto.CartList)
            {
                OrderProduct orderProduct = new OrderProduct
                {
                    OrderId = orderMap.Id,
                    ProductId = cartItem.ProductID,
                    Quantity = cartItem.Qty,
                    Subtotal = cartItem.SubTotal
                };
                orderProducts.Add(orderProduct);
                var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartItem.ProductID);
                if (product != null)
                {
                    product.StockQuantity -= cartItem.Qty;
                }
                _context.Products!.Update(product);
            }
            _context.OrderProducts.AddRange(orderProducts);

            await _context.SaveChangesAsync();

            return orderMap;
        }

        public async Task UpdateOrderAsync(OrderCreateDto orderDto)
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
