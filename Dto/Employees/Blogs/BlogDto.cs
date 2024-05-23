
namespace MediNet_BE.Dto.Employees.Blogs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
		public string Image { get; set; }
		public string ImageSrc { get; set; } = string.Empty;
		public string Content { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeId { get; set; }
        public int DiseaseId { get; set; }
        public EmployeeDto Employee { get; set; }
        public DiseaseDto Disease { get; set; }
		public ICollection<BlogCommentDto>? BlogComments { get; set; }
	}
}
