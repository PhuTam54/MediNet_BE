using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.Interfaces.Employees.Courses;
using MediNet_BE.Models.Employees.Courses;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees.Courses
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
                .Include(e => e.Employee)
                .ToListAsync();

            var coursesMap = _mapper.Map<List<CourseDto>>(courses);

            return coursesMap;
        }

        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses!
                .Include(e => e.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            var courseMap = _mapper.Map<CourseDto>(course);

            return courseMap;
        }

		public async Task<List<CourseDto>> GetCoursesByEmployeeIdAsync(int employeeId)
		{
			var courses = await _context.Enrollments
				.Include(e => e.Employee)
                .Where(e => e.Employee.Id == employeeId)
                .Select(c => c.Course)
				.ToListAsync();

			var coursesMap = _mapper.Map<List<CourseDto>>(courses);

			return coursesMap;
		}

		public async Task<List<CourseDto>> GetCoursesByDoctorIdAsync(int doctorId)
		{
			var courses = await _context.Courses
				.Include(e => e.Employee)
				.Where(e => e.Employee.Id == doctorId)
				.ToListAsync();

			var coursesMap = _mapper.Map<List<CourseDto>>(courses);

			return coursesMap;
		}

		public async Task<Course> AddCourseAsync(CourseCreate courseCreate)
        {
            var employeeDoctor = await _context.Employees.FirstOrDefaultAsync(d => d.Id == courseCreate.EmployeeId);
            var courseMap = _mapper.Map<Course>(courseCreate);
            courseMap.CreatedAt = DateTime.UtcNow;
            courseMap.Employee = employeeDoctor;

            _context.Courses!.Add(courseMap);
            await _context.SaveChangesAsync();
            return courseMap;
        }

        public async Task UpdateCourseAsync(CourseCreate courseCreate)
        {
            var employeeDoctor = await _context.Employees.FirstOrDefaultAsync(d => d.Id == courseCreate.EmployeeId);
            var courseMap = _mapper.Map<Course>(courseCreate);
            courseMap.CreatedAt = DateTime.UtcNow;
            courseMap.Employee = employeeDoctor;

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
