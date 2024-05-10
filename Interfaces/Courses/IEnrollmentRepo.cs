using MediNet_BE.Dto.Courses;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.Models.Courses;

namespace MediNet_BE.Interfaces.Enrollments
{
	public interface IEnrollmentRepo
	{
		public Task<List<EnrollmentDto>> GetAllEnrollmentAsync();
		public Task<EnrollmentDto> GetEnrollmentByIdAsync(int id);
		public Task<Enrollment> AddEnrollmentAsync(EnrollmentCreate enrollmentCreate);
		public Task UpdateEnrollmentAsync(EnrollmentCreate enrollmentCreate);
		public Task DeleteEnrollmentAsync(int id);
	}
}
