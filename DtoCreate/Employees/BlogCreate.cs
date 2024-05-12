namespace MediNet_BE.DtoCreate.Employees
{
    public class BlogCreate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int EmployeeId { get; set; }
        public int DiseaseId { get; set; }
    }
}
