using AutoMapper;
using MediNet_BE.Controllers.Users;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Employees;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Users;
using MediNet_BE.Services;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Users
{
	public class CustomerRepo : IUserRepo<Customer, CustomerDto, CustomerCreate>
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
		public async Task<List<CustomerDto>> GetAllUserAsync()
		{
			var customers = await _context.Customers!
				.Include(o => o.Orders)
				.Include(f => f.Feedbacks)
				.Include(c => c.Carts)
				.ToListAsync();

			var customersMap = _mapper.Map<List<CustomerDto>>(customers);

			return customersMap;
		}

		public async Task<CustomerDto> GetUserByIdAsync(int id)
		{
			var customer = await _context.Customers!
								.Include(o => o.Orders)
								.Include(f => f.Feedbacks)
								.Include(c => c.Carts)
								.AsNoTracking()
								.FirstOrDefaultAsync(c => c.Id == id);
			var customerMap = _mapper.Map<CustomerDto>(customer);

			return customerMap;
		}

		public async Task<CustomerDto> GetUserByEmailAsync(string email)
		{
			var customer = await _context.Customers!
				.Include(o => o.Orders)
				.Include(f => f.Feedbacks)
				.Include(c => c.Carts)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Email == email);

			var customerMap = _mapper.Map<CustomerDto>(customer);

			return customerMap;
		}

		public async Task<Customer> AddUserAsync(CustomerCreate userCreate)
		{
			var customerMap = _mapper.Map<Customer>(userCreate);
			customerMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);
			customerMap.Role = 1;
			customerMap.Status = 0;
			customerMap.Date_Of_Birth = DateTime.UtcNow;
			customerMap.Password = LoginRegisterController.HashPassword(customerMap.Password);

			var data = new SendMailRequest
			{
				ToEmail = customerMap.Email,
				UserName = customerMap.Username,
				Url = "verifyaccount",
				Subject = $"Verify Account for {customerMap.Username}"
			};
			await _mailService.SendEmailAsync(data);

			_context.Customers!.Add(customerMap);
			await _context.SaveChangesAsync();
			return customerMap;
		}

		public async Task UpdateUserAsync(CustomerCreate userCreate)
		{
			var customerMap = _mapper.Map<Customer>(userCreate);
			customerMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);

			_context.Customers!.Update(customerMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(int userId)
		{
			var customer = await _context.Customers!.FirstOrDefaultAsync(c => c.Id == userId);

			_context.Customers!.Remove(customer);
			await _context.SaveChangesAsync();
		}

		public async Task<List<CustomerDto>> GetUserByTotalAmountOrderAsync()
		{
			var customers = await _context.Customers!
		.Include(o => o.Orders)
		.Include(f => f.Feedbacks)
		.Include(c => c.Carts)
		.Select(c => new
		{
			Customer = c,
			TotalOrderAmount = c.Orders.Sum(o => o.TotalAmount)
		})
		.OrderByDescending(c => c.TotalOrderAmount)
		.ToListAsync();

			var customersMap = _mapper.Map<List<CustomerDto>>(customers.Select(c => c.Customer).ToList());

			return customersMap;
		}

	}
}
