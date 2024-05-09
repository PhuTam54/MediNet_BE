using MediNet_BE.Models.Users;

namespace MediNet_BE.Models.Courses
{
    public class Enrollment
	{
		public int Id { get; set; }
		public DateTime EnrolledAt { get; set; }
		public int CourseId { get; set; }
		public int EmployeeId { get; set; }
		public Course Course { get; set;}
		public Employee Employee { get; set; }
	}
}
