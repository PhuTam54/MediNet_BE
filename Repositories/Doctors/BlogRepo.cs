using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Doctors;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Doctors
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
            var blogs = await _context.Blogs!
                .Include(d => d.Doctor)
                .Include(d => d.Disease)
                .ToListAsync();

            var blogsMap = _mapper.Map<List<BlogDto>>(blogs);

            return blogsMap;
        }

        public async Task<BlogDto> GetBlogByIdAsync(int id)
        {
            var blog = await _context.Blogs!
				.Include(d => d.Doctor)
				.Include(d => d.Disease)
				.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var blogMap = _mapper.Map<BlogDto>(blog);

            return blogMap;
        }

        public async Task<Blog> AddBlogAsync(BlogCreate blogCreate)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == blogCreate.DoctorId);
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == blogCreate.DiseaseId);
            var blogMap = _mapper.Map<Blog>(blogCreate);
            blogMap.CreatedAt = DateTime.UtcNow;
            blogMap.Doctor = doctor;
            blogMap.Disease = disease;

            _context.Blogs!.Add(blogMap);
            await _context.SaveChangesAsync();
            return blogMap;
        }

        public async Task UpdateBlogAsync(BlogCreate blogCreate)
        {
            var doctor = await _context.Doctors!.FirstOrDefaultAsync(d => d.Id == blogCreate.DoctorId);
            var disease = await _context.Diseases!.FirstOrDefaultAsync(d => d.Id == blogCreate.DiseaseId);
            var blogMap = _mapper.Map<Blog>(blogCreate);
			blogMap.CreatedAt = DateTime.UtcNow;
			blogMap.Doctor = doctor;
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
