using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class CategoryChildRepo : ICategoryChildRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CategoryChildRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<CategoryChild>> GetAllCategoryChildAsync()
		{
			var categoryChilds = await _context.CategoryChilds!.ToListAsync();
			return categoryChilds;
		}

		public async Task<CategoryChild> GetCategoryChildByIdAsync(int id)
		{
			var categoryChild = await _context.CategoryChilds!.AsNoTracking().FirstOrDefaultAsync(cc => cc.Id == id);
			return categoryChild;
		}

		public async Task<CategoryChild> AddCategoryChildAsync(CategoryChildDto categoryChildDto)
		{
			var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildDto.CategoryId);
			var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildDto);
			categoryChildMap.Category = catetory;
			_context.CategoryChilds!.Add(categoryChildMap);
			await _context.SaveChangesAsync();
			return categoryChildMap;
		}

		public async Task UpdateCategoryChildAsync(CategoryChildDto categoryChildDto)
		{
			var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildDto.CategoryId);
			var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildDto);
			categoryChildMap.Category = catetory;

			_context.CategoryChilds!.Update(categoryChildMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCategoryChildAsync(CategoryChild categoryChild)
		{
			_context.CategoryChilds!.Remove(categoryChild);
			await _context.SaveChangesAsync();
		}	
	}
}
