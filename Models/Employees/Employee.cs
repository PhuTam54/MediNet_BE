using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Models.Employees
{
    public class Employee : User
    {
		public string Full_Name { get; set; }
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public int SpecialistId { get; set; }
		public int ClinicId { get; set; }
		public Specialist Specialist { get; set; }
		public Clinic Clinic { get; set; }
		public ICollection<Blog>? Blogs { get; set; }
		public ICollection<Service>? Services { get; set; }
		public ICollection<Course>? Courses { get; set; }
		public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
