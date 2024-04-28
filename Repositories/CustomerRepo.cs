using AutoMapper;
using MediNet_BE.Controllers.Users;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Users;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Models.Users;
using MediNet_BE.Services;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class CustomerRepo : IUserRepo<Customer, CustomerDto>
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;
		private readonly IMailService _mailService;

		public CustomerRepo(MediNetContext context, IMapper mapper, IMailService mailService)
		{
			_context = context;
			_mapper = mapper;
			_mailService = mailService;
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
			customerMap.Role = 1;
			customerMap.Status = 0;
			customerMap.Date_Of_Birth = DateTime.UtcNow;
			customerMap.Password = LoginRegisterController.HashPassword(customerMap.Password);

			var data = new SendMailRequest
			{
				ToEmail = customerMap.Email,
				UserName = customerMap.Username,
				Url = "ThankYou",
				Subject = $"Thank you {customerMap.Username}"
			};
			await _mailService.SendEmailAsync(data);

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
