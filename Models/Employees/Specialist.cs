
namespace MediNet_BE.Models.Employees
{
    public class Specialist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
