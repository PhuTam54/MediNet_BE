using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Employees.Blogs
{
	public class BlogCommentRepo : IBlogCommentRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public BlogCommentRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<BlogCommentDto>> GetAllBlogCommentAsync()
		{
			var blogComments = await _context.BlogComments!
				.Include(b => b.Blog)
				.Include(c => c.Customer)
				.ToListAsync();

			var blogCommentsMap = _mapper.Map<List<BlogCommentDto>>(blogComments);

			return blogCommentsMap;
		}

		public async Task<BlogCommentDto> GetBlogCommentByIdAsync(int id)
		{
			var blogComment = await _context.BlogComments
				.Include(b => b.Blog)
				.Include(c => c.Customer)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var blogCommentMap = _mapper.Map<BlogCommentDto>(blogComment);

			return blogCommentMap;
		}

		public async Task<List<BlogCommentDto>> GetBlogCommentByParentIdAsync(int parentId)
		{
			var blogComments = await _context.BlogComments
				.Include(b => b.Blog)
				.Include(c => c.Customer)
				.Where(bc => bc.Parent_Id == parentId)
				.ToListAsync();

			var blogCommentsMap = _mapper.Map<List<BlogCommentDto>>(blogComments);

			return blogCommentsMap;
		}

		public async Task<List<BlogCommentDto>> GetBlogCommentByBlogIdAsync(int blogId)
		{
			var blogComments = await _context.BlogComments
				.Include(b => b.Blog)
				.Include(c => c.Customer)
				.Where(bm => bm.Blog.Id == blogId)
				.ToListAsync();

			var blogCommentsMap = _mapper.Map<List<BlogCommentDto>>(blogComments);

			return blogCommentsMap;
		}

		public async Task<BlogComment> AddBlogCommentAsync(BlogCommentCreate blogCommentCreate)
		{
			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == blogCommentCreate.CustomerId);
			var blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == blogCommentCreate.BlogId);

			var blogCommentMap = _mapper.Map<BlogComment>(blogCommentCreate);
			blogCommentMap.Customer = customer;
			blogCommentMap.Blog = blog;
			blogCommentMap.CreatedAt = DateTime.UtcNow;
			blogCommentMap.LastUpdatedAt = DateTime.UtcNow;

			_context.BlogComments!.Add(blogCommentMap);
			await _context.SaveChangesAsync();
			return blogCommentMap;
		}

		public async Task UpdateBlogCommentAsync(BlogCommentCreate blogCommentCreate)
		{
			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == blogCommentCreate.CustomerId);
			var blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == blogCommentCreate.BlogId);

			var blogCommentMap = _mapper.Map<BlogComment>(blogCommentCreate);
			blogCommentMap.Customer = customer;
			blogCommentMap.Blog = blog;
			blogCommentMap.LastUpdatedAt = DateTime.UtcNow;

			_context.BlogComments!.Update(blogCommentMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteBlogCommentAsync(int id)
		{
			var blogComment = await _context.BlogComments!.FirstOrDefaultAsync(c => c.Id == id);
			
			var blogCommentChilds = await _context.BlogComments!
				.Include(b => b.Blog)
				.Include(c => c.Customer)
				.Where(bc => bc.Parent_Id == id)
				.ToListAsync();
			foreach (var blogCommentChild in blogCommentChilds)
			{
				_context.BlogComments!.Remove(blogCommentChild);
			}
			_context.BlogComments!.Remove(blogComment);
			await _context.SaveChangesAsync();
		}

		
	}
}
