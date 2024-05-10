using AutoMapper;
using MediNet_BE.Controllers.Users;
using MediNet_BE.Data;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Doctors
{
	public class DoctorRepo : IUserRepo<Doctor, DoctorDto ,DoctorCreate>
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public DoctorRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<DoctorDto>> GetAllUserAsync()
		{
			var doctors = await _context.Doctors!
				.Include(s => s.Specialist)
				.Include(c => c.Clinic)
				.ToListAsync();
			var doctorsMap = _mapper.Map<List<DoctorDto>>(doctors);

			return doctorsMap;
		}

		public async Task<DoctorDto> GetUserByIdAsync(int id)
		{
			var doctor = await _context.Doctors!
				.Include(s => s.Specialist)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var doctorMap = _mapper.Map<DoctorDto>(doctor);

			return doctorMap;
		}

		public async Task<DoctorDto> GetUserByEmailAsync(string email)
		{
			var doctor = await _context.Doctors!
				.Include(s => s.Specialist)
				.Include(c => c.Clinic)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Email == email);
			var doctorMap = _mapper.Map<DoctorDto>(doctor);

			return doctorMap;
		}

		public async Task<Doctor> AddUserAsync(DoctorCreate userCreate)
		{
			var doctorMap = _mapper.Map<Doctor>(userCreate);
			doctorMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);
			doctorMap.Password = LoginRegisterController.HashPassword(doctorMap.Password);
			doctorMap.Role = 3;

			_context.Doctors!.Add(doctorMap);
			await _context.SaveChangesAsync();
			return doctorMap;
		}

		public async Task UpdateUserAsync(DoctorCreate userCreate)
		{
			var doctorMap = _mapper.Map<Doctor>(userCreate);
			doctorMap.SEO_Name = CreateSlug.Init_Slug(userCreate.Username);
			doctorMap.Password = LoginRegisterController.HashPassword(doctorMap.Password);
			doctorMap.Role = 3;

			_context.Doctors!.Update(doctorMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(int userId)
		{
			var doctor = await _context.Doctors!.FirstOrDefaultAsync(c => c.Id == userId);

			_context.Doctors!.Remove(doctor);
			await _context.SaveChangesAsync();
		}
	}
}
