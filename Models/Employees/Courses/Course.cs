using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models.Employees.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
		public string ImagesCourse { get; set; }
		public string Description { get; set; }
        public string Duration { get; set; }
        public string Location { get; set; }
        public string Topics { get; set; }
        public string TargetAudience { get; set; }
        public string SkillCovered { get; set; }
        public bool MedicineSalesTraining { get; set; }
        public bool MedicalExaminationTraining { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
