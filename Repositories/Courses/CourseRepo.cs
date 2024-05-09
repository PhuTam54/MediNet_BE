using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Courses;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Courses;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Courses;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Courses
{
    public class CourseRepo : ICourseRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CourseRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<CourseDto>> GetAllCourseAsync()
		{
			var courses = await _context.Courses!
				.ToListAsync();

			var coursesMap = _mapper.Map<List<CourseDto>>(courses);

			return coursesMap;
		}

		public async Task<CourseDto> GetCourseByIdAsync(int id)
		{
			var course = await _context.Courses!
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);
			var courseMap = _mapper.Map<CourseDto>(course);

			return courseMap;
		}

		public async Task<Course> AddCourseAsync(CourseCreate courseCreate)
		{
			var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == courseCreate.DoctorId);
			var courseMap = _mapper.Map<Course>(courseCreate);
			courseMap.CreatedAt = DateTime.UtcNow;
			courseMap.Doctor = doctor;

			_context.Courses!.Add(courseMap);
			await _context.SaveChangesAsync();
			return courseMap;
		}

		public async Task UpdateCourseAsync(CourseCreate courseCreate)
		{
			var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == courseCreate.DoctorId);
			var courseMap = _mapper.Map<Course>(courseCreate);
			courseMap.CreatedAt = DateTime.UtcNow;
			courseMap.Doctor = doctor;

			_context.Courses!.Update(courseMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCourseAsync(int id)
		{
			var course = await _context.Courses!.SingleOrDefaultAsync(c => c.Id == id);

			_context.Courses!.Remove(course);
			await _context.SaveChangesAsync();
		}
	}
}
