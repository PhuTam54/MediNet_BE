namespace MediNet_BE.DtoCreate.Doctors
{
    public class BlogCreate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int DoctorId { get; set; }
        public int DiseaseId { get; set; }
    }
}
