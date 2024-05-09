using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Courses;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Courses;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Dto.Doctors
{
	public class DoctorDto : UserDto
	{
		public string Full_Name { get; set; }
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public int PositionId { get; set; }
		public int SpecialistId { get; set; }
		public int ClinicId { get; set; }
		public SpecialistDto Specialist { get; set; }
		public ClinicDto Clinic { get; set; }
		public ICollection<BlogDto>? Blogs { get; set; }
		public ICollection<ServiceDto>? Services { get; set; }
		public ICollection<CourseDto>? Courses { get; set; }
	}
}
