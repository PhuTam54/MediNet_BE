using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Dto.Employees
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeId { get; set; }
        public int DiseaseId { get; set; }
        public EmployeeDto Employee { get; set; }
        public DiseaseDto Disease { get; set; }
    }
}
