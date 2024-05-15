using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.Interfaces.Employees.Courses;
using MediNet_BE.Models.Employees.Courses;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees.Courses
{
    public class EnrollmentRepo : IEnrollmentRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public EnrollmentRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EnrollmentDto>> GetAllEnrollmentAsync()
        {
            var enrollments = await _context.Enrollments!
                .Include(e => e.Employee)
                .Include(c => c.Course)
                .ToListAsync();

            var enrollmentsMap = _mapper.Map<List<EnrollmentDto>>(enrollments);

            return enrollmentsMap;
        }

        public async Task<EnrollmentDto> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.Enrollments!
                .Include(e => e.Employee)
                .Include(c => c.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            var enrollmentMap = _mapper.Map<EnrollmentDto>(enrollment);

            return enrollmentMap;
        }

        public async Task<Enrollment> AddEnrollmentAsync(EnrollmentCreate enrollmentCreate)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == enrollmentCreate.CourseId);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == enrollmentCreate.EmployeeId);
            var enrollmentMap = _mapper.Map<Enrollment>(enrollmentCreate);
            enrollmentMap.EnrolledAt = DateTime.UtcNow;
            enrollmentMap.Course = course;
            enrollmentMap.Employee = employee;

            _context.Enrollments!.Add(enrollmentMap);
            await _context.SaveChangesAsync();
            return enrollmentMap;
        }

        public async Task UpdateEnrollmentAsync(EnrollmentCreate enrollmentCreate)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == enrollmentCreate.CourseId);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == enrollmentCreate.EmployeeId);
            var enrollmentMap = _mapper.Map<Enrollment>(enrollmentCreate);
            enrollmentMap.EnrolledAt = DateTime.UtcNow;
            enrollmentMap.Course = course;
            enrollmentMap.Employee = employee;

            _context.Enrollments!.Update(enrollmentMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.Enrollments!.SingleOrDefaultAsync(c => c.Id == id);

            _context.Enrollments!.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
