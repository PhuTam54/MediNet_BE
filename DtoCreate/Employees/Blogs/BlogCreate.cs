namespace MediNet_BE.DtoCreate.Employees.Blogs
{
    public class BlogCreate
    {
        public int Id { get; set; }
        public string Title { get; set; }
		public string Image { get; set; } = string.Empty;
		public IFormFile? ImageFile { get; set; }
		public string Content { get; set; }
        public int Status { get; set; }
        public int EmployeeId { get; set; }
        public int DiseaseId { get; set; }
    }
}
