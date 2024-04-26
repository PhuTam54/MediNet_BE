using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Users;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class CustomerRepo : IUserRepo<Customer, CustomerDto>
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CustomerRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<List<Customer>> GetAllUserAsync()
		{
			var customers = await _context.Customers!.ToListAsync();
			return customers;
		}

		public async Task<Customer> GetUserByIdAsync(int id)
		{
			var customer = await _context.Customers!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return customer;
		}

		public async Task<Customer> AddUserAsync(CustomerDto userDto)
		{
			var customerMap = _mapper.Map<Customer>(userDto);
			_context.Customers!.Add(customerMap);
			await _context.SaveChangesAsync();
			return customerMap;
		}

		public async Task UpdateUserAsync(CustomerDto user)
		{
			var customerMap = _mapper.Map<Customer>(user);
			_context.Customers!.Update(customerMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(Customer user)
		{
			_context.Customers!.Remove(user);
			await _context.SaveChangesAsync();
		}
	}
}
