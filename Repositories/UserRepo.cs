using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Users;
using MediNet_BE.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
    public class UserRepo 
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public UserRepo(MediNetContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		//public async Task<List<User>> getAllUserAsync()
		//{
		//	var users = await _context.Users!.ToListAsync();
		//	return users;
		//}

		//public async Task<User> getUserByIdAsync(int id)
		//{
		//	var user = await _context.Users!.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
		//	return user;
		//}

		//public async Task<User> AddUserAsync(UserDto userDto)
		//{
		//	var userMap = _mapper.Map<User>(userDto);
		//	_context.Users!.Add(userMap);
		//	await _context.SaveChangesAsync();
		//	return userMap;
		//}

		//public async Task UpdateUserAsync(UserDto userDto)
		//{
		//	var userMap = _mapper.Map<User>(userDto);
		//	_context.Users!.Update(userMap);
		//	await _context.SaveChangesAsync();
		//}

		//public async Task DeleteUserAsync(User user)
		//{
		//	_context.Users!.Remove(user);
		//	await _context.SaveChangesAsync();
		//}


	}
}
