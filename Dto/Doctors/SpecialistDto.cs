using MediNet_BE.Dto.Courses;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Dto.Doctors
{
    public class SpecialistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<EmployeeDto>? Employees { get; set; }
    }
}
