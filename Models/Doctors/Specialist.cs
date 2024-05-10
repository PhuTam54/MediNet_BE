using MediNet_BE.Models.Courses;

namespace MediNet_BE.Models.Doctors
{
    public class Specialist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }
        public ICollection<Employee>? Employees { get; set; }

    }
}
