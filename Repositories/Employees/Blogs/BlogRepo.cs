using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Employees.Blogs;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees.Blogs
{
    public class BlogRepo : IBlogRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public BlogRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BlogDto>> GetAllBlogAsync()
        {
            var blogs = await _context.Blogs
                .Include(e => e.Employee)
                .Include(d => d.Disease)
                .Include(bm => bm.BlogComments)
                .ToListAsync();

            var blogsMap = _mapper.Map<List<BlogDto>>(blogs);

            return blogsMap;
        }

        public async Task<BlogDto> GetBlogByIdAsync(int id)
        {
            var blog = await _context.Blogs
                .Include(e => e.Employee)
                .Include(d => d.Disease)
				.Include(bm => bm.BlogComments)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var blogMap = _mapper.Map<BlogDto>(blog);

            return blogMap;
        }

		public async Task<List<BlogDto>> GetBlogsByEmployeeIdAsync(int employeeId)
		{
			var blogs = await _context.Blogs
				.Include(e => e.Employee)
				.Include(d => d.Disease)
				.Include(bm => bm.BlogComments)
                .Where(b => b.Employee.Id == employeeId)
				.ToListAsync();

			var blogsMap = _mapper.Map<List<BlogDto>>(blogs);

			return blogsMap;
		}

		public async Task<List<BlogDto>> GetBlogsByDiseaseIdAsync(int diseaseId)
		{
			var blogs = await _context.Blogs
				.Include(e => e.Employee)
				.Include(d => d.Disease)
			    .Include(bm => bm.BlogComments)
				.Where(b => b.Disease.Id == diseaseId)
				.ToListAsync();

			var blogsMap = _mapper.Map<List<BlogDto>>(blogs);

			return blogsMap;
		}

		public async Task<Blog> AddBlogAsync(BlogCreate blogCreate)
        {
            var employeeDoctor = await _context.Employees.FirstOrDefaultAsync(d => d.Id == blogCreate.EmployeeId);
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == blogCreate.DiseaseId);
            var blogMap = _mapper.Map<Blog>(blogCreate);
            blogMap.CreatedAt = DateTime.UtcNow;
            blogMap.Employee = employeeDoctor;
            blogMap.Disease = disease;

            _context.Blogs!.Add(blogMap);
            await _context.SaveChangesAsync();
            return blogMap;
        }

        public async Task UpdateBlogAsync(BlogCreate blogCreate)
        {
            var employeeDoctor = await _context.Employees!.FirstOrDefaultAsync(d => d.Id == blogCreate.EmployeeId);
            var disease = await _context.Diseases!.FirstOrDefaultAsync(d => d.Id == blogCreate.DiseaseId);
            var blogMap = _mapper.Map<Blog>(blogCreate);
            blogMap.CreatedAt = DateTime.UtcNow;
            blogMap.Employee = employeeDoctor;
            blogMap.Disease = disease;

            _context.Blogs!.Update(blogMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await _context.Blogs!.FirstOrDefaultAsync(c => c.Id == id);
            _context.Blogs!.Remove(blog);
            await _context.SaveChangesAsync();
        }

		
	}
}
