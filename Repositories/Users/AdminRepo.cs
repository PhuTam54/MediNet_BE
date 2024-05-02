using AutoMapper;
using MediNet_BE.Controllers.Users;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Users;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Users
{
    public class AdminRepo : IUserRepo<Admin, AdminDto>
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public AdminRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Admin>> GetAllUserAsync()
        {
            var admins = await _context.Admins!.ToListAsync();

			return admins;
        }

        public async Task<Admin> GetUserByIdAsync(int id)
        {
            var admin = await _context.Admins!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

			return admin;
        }

        public async Task<Admin> GetUserByEmailAsync(string email)
        {
            var admin = await _context.Admins!.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
			return admin;
        }

        public async Task<Admin> AddUserAsync(AdminDto userDto)
        {
            var adminMap = _mapper.Map<Admin>(userDto);
            adminMap.SEO_Name = CreateSlug.Init_Slug(userDto.Username);
            adminMap.Password = LoginRegisterController.HashPassword(adminMap.Password);
            adminMap.Role = 2;

            _context.Admins!.Add(adminMap);
            await _context.SaveChangesAsync();
            return adminMap;
        }

        public async Task UpdateUserAsync(AdminDto userDto)
        {
            var adminMap = _mapper.Map<Admin>(userDto);
            adminMap.SEO_Name = CreateSlug.Init_Slug(userDto.Username);
            adminMap.Password = LoginRegisterController.HashPassword(adminMap.Password);
            adminMap.Role = 2;

            _context.Admins!.Update(adminMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
			var admin = await _context.Admins!.FirstOrDefaultAsync(c => c.Id == userId);

			_context.Admins!.Remove(admin);
            await _context.SaveChangesAsync();
        }


    }
}
