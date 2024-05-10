namespace MediNet_BE.Models.Doctors
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DoctorId { get; set; }
        public int DiseaseId { get; set; }
        public Doctor Doctor { get; set; }
        public Disease Disease { get; set; }
    }
}
