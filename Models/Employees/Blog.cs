
namespace MediNet_BE.Models.Employees
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeId { get; set; }
        public int DiseaseId { get; set; }
        public Employee Employee { get; set; }
        public Disease Disease { get; set; }
    }
}
