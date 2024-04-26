using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class CategoryRepo : ICategoryRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CategoryRepo(MediNetContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Category>> GetAllCategoryAsync()
		{
			var categories = await _context.Categories!.Include(cc => cc.CategoryChilds).ToListAsync();
			return categories;
		}

		public async Task<Category> GetCategoryByIdAsync(int id)
		{
			var category = await _context.Categories!.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return category;
		}
		
		public async Task<Category> AddCategoryAsync(CategoryDto categoryDto)
		{
			var categoryMap = _mapper.Map<Category>(categoryDto);
			_context.Categories!.Add(categoryMap);
			await _context.SaveChangesAsync();
			return categoryMap;
		}

		public async Task UpdateCategoryAsync(CategoryDto categoryDto)
		{
			var categoryMap = _mapper.Map<Category>(categoryDto);
			_context.Categories!.Update(categoryMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCategoryAsync(Category category)
		{
			_context.Categories!.Remove(category);
			await _context.SaveChangesAsync();
		}

	}
}
