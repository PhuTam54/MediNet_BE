using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
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

		public async Task<List<CartDto>> GetCartsByCustomerIdAsync(int customerId)
		{
			var carts = await _context.Carts
				.Include(c => c.Customer)
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.Where(c => c.Customer.Id == customerId).ToListAsync();
            var cartsMap = _mapper.Map<List<CartDto>>(carts);

            return cartsMap;
        }

		public async Task<CartDto> GetCartByIdAsync(int id)
		{
			var cart = await _context.Carts!
				.Include(c => c.Customer)
				.Include(p => p.Product)
				.Include(c => c.Clinic)
				.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

			var cartMap = _mapper.Map<CartDto>(cart);

			return cartMap;
		}

		public async Task<Cart> AddCartAsync(CartCreate cartCreate)
		{
			var customer = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartCreate.CustomerId);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == cartCreate.ClinicId);

			var cartMap = _mapper.Map<Cart>(cartCreate);
			cartMap.Customer = customer;
			cartMap.Product = product;
			cartMap.Clinic = clinic;
			cartMap.SubTotal = Math.Round(product.Price * cartCreate.QtyCart, 2);

			_context.Carts!.Add(cartMap);
			await _context.SaveChangesAsync();
			return cartMap;
		}

		public async Task UpdateCartAsync(CartCreate cartCreate)
		{
			var customer = await _context.Customers!.FirstOrDefaultAsync(u => u.Id == cartCreate.CustomerId);
			var product = await _context.Products!.FirstOrDefaultAsync(p => p.Id == cartCreate.ProductId);
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == cartCreate.ClinicId);

			var cartMap = _mapper.Map<Cart>(cartCreate);
			cartMap.Customer = customer;
			cartMap.Product = product;
			cartMap.Clinic = clinic;

			cartMap.SubTotal = Math.Round(product.Price * cartCreate.QtyCart, 2);

			_context.Carts!.Update(cartMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCartAsync(int id)
		{
			var cart = await _context.Carts!.FirstOrDefaultAsync(c => c.Id == id);
			_context.Carts!.Remove(cart);
			await _context.SaveChangesAsync();
		}

		public async Task<Cart> CheckCartExist(int productId, int clinicId, int customerId)
		{
            var cart = await _context.Carts.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Customer.Id == customerId && c.Product.Id == productId && c.Clinic.Id == clinicId);
            return cart;
        }
	}
}
