using AutoMapper;
using MediNet_BE.Controllers.Users;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Courses
{
    public class EmployeeRepo : IUserRepo<Employee, EmployeeDto, EmployeeCreate>
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetAllUserAsync()
        {
            var employees = await _context.Employees
                .Include(s => s.Specialist)
                .Include(c => c.Clinic)
                .Include(c => c.Courses)
				.Include(e => e.Enrollments)
				.Include(b => b.Blogs)
				.Include(s => s.Services)
				.ToListAsync();
            var employeesMap = _mapper.Map<List<EmployeeDto>>(employees);

            return employeesMap;
        }

        public async Task<EmployeeDto> GetUserByIdAsync(int id)
        {
            var employee = await _context.Employees
				.Include(s => s.Specialist)
				.Include(c => c.Clinic)
				.Include(c => c.Courses)
				.Include(e => e.Enrollments)
				.Include(b => b.Blogs)
				.Include(s => s.Services)
				.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            var employeeMap = _mapper.Map<EmployeeDto>(employee);

            return employeeMap;
        }

        public async Task<EmployeeDto> GetUserByEmailAsync(string email)
        {
            var employee = await _context.Employees
				.Include(s => s.Specialist)
				.Include(c => c.Clinic)
				.Include(c => c.Courses)
				.Include(e => e.Enrollments)
				.Include(b => b.Blogs)
				.Include(s => s.Services)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email);
            var employeeMap = _mapper.Map<EmployeeDto>(employee);

            return employeeMap;
        }

        public async Task<Employee> AddUserAsync(EmployeeCreate userCreate)
        {
            var specialist = await _context.Specialists.FirstOrDefaultAsync(s => s.Id == userCreate.SpecialistId);
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id ==  userCreate.ClinicId);
            var employeeMap = _mapper.Map<Employee>(userCreate);
            employeeMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);
            employeeMap.Password = LoginRegisterController.HashPassword(employeeMap.Password);
            employeeMap.Role = 4;
            employeeMap.Specialist = specialist;
            employeeMap.Clinic = clinic;

            _context.Employees!.Add(employeeMap);
            await _context.SaveChangesAsync();
            return employeeMap;
        }

        public async Task UpdateUserAsync(EmployeeCreate userCreate)
        {
			var specialist = await _context.Specialists.FirstOrDefaultAsync(s => s.Id == userCreate.SpecialistId);
			var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == userCreate.ClinicId);
			var employeeMap = _mapper.Map<Employee>(userCreate);
            employeeMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);
            employeeMap.Password = LoginRegisterController.HashPassword(employeeMap.Password);
            employeeMap.Role = 4;
			employeeMap.Specialist = specialist;
			employeeMap.Clinic = clinic;

			_context.Employees!.Update(employeeMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var employee = await _context.Employees!.FirstOrDefaultAsync(c => c.Id == userId);

            _context.Employees!.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
