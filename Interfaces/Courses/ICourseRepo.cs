
using MediNet_BE.Dto.Courses;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.Models.Courses;

namespace MediNet_BE.Interfaces.Courses
{
    public interface ICourseRepo
	{
		public Task<List<CourseDto>> GetAllCourseAsync();
		public Task<CourseDto> GetCourseByIdAsync(int id);
		public Task<Course> AddCourseAsync(CourseCreate courseCreate);
		public Task UpdateCourseAsync(CourseCreate courseCreate);
		public Task DeleteCourseAsync(int id);
	}
}
