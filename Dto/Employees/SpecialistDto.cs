namespace MediNet_BE.Dto.Employees
{
    public class SpecialistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<EmployeeDto>? Employees { get; set; }
    }
}
