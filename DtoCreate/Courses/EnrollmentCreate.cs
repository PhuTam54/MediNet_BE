namespace MediNet_BE.DtoCreate.Courses
{
	public class EnrollmentCreate
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int EmployeeId { get; set; }
	}
}
