namespace MediNet_BE.Dto.Employees.Blogs
{
    public class DiseaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<BlogDto>? Blogs { get; set; }
    }
}
