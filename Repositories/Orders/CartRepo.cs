using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
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

		public async Task<List<CartReturnDto>> GetCartsByCustomerIdAsync(int customerId)
		{
			var carts = await _context.Carts
				.Include(c => c.Customer)
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.Where(c => c.Customer.Id == customerId).ToListAsync();
            var cartMap = _mapper.Map<List<CartReturnDto>>(carts);

            return cartMap;
        }

		public async Task<Cart> GetCartByIdAsync(int id)
		{
			var cart = await _context.Carts!
				.Include(c => c.Customer)
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return cart;
		}

		public async Task<Cart> AddCartAsync(CartDto cartDto)
		{
			var customer = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartDto.CustomerID);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartDto.ProductID);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id ==  cartDto.ClinicID);

			var cartMap = _mapper.Map<Cart>(cartDto);
			cartMap.Customer = customer;
			cartMap.Product = product;
			cartMap.Clinic = clinic;
			cartMap.SubTotal = Math.Round(product.Price * cartDto.QtyCart, 2);

			_context.Carts!.Add(cartMap);
			await _context.SaveChangesAsync();
			return cartMap;
		}

		public async Task UpdateCartAsync(CartDto cartDto)
		{
			var customer = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartDto.CustomerID);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartDto.ProductID);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == cartDto.ClinicID);

			var cartMap = _mapper.Map<Cart>(cartDto);
			cartMap.Customer = customer;
			cartMap.Product = product;
			cartMap.Clinic = clinic;

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
