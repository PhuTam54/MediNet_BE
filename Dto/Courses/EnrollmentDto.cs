using MediNet_BE.Models.Courses;

namespace MediNet_BE.Dto.Courses
{
    public class EnrollmentDto
    {
		public int Id { get; set; }
		public DateTime EnrolledAt { get; set; }
		public int CourseId { get; set; }
		public int EmployeeId { get; set; }
		public CourseDto Course { get; set; }
		public EmployeeDto Employee { get; set; }
	}
}
