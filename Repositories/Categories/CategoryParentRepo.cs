using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Categories
{
    public class CategoryParentRepo : ICategoryParentRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public CategoryParentRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<CategoryParentDto>> GetAllCategoryParentAsync()
		{
			var categoryParents = await _context.CategoryParents
				.Include(c => c.Categories)
				.ThenInclude(cc => cc.CategoryChilds)
				.ThenInclude(p => p.Products)
				.ToListAsync();

			var categoryParentsMap = _mapper.Map<List<CategoryParentDto>>(categoryParents);

			return categoryParentsMap;
		}

		public async Task<CategoryParentDto> GetCategoryParentByIdAsync(int id)
		{
			var categoryParent = await _context.CategoryParents
				.Include(c => c.Categories)
				.ThenInclude(cc => cc.CategoryChilds)
				.ThenInclude(p => p.Products)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			var categoryParentMap = _mapper.Map<CategoryParentDto>(categoryParent);

			return categoryParentMap;
		}

		public async Task<CategoryParent> AddCategoryParentAsync(CategoryParentCreate categoryParentCreate)
		{
			var categoryParentMap = _mapper.Map<CategoryParent>(categoryParentCreate);
			categoryParentMap.Slug = CreateSlug.Init_Slug(categoryParentCreate.Name);

			_context.CategoryParents!.Add(categoryParentMap);
			await _context.SaveChangesAsync();
			return categoryParentMap;
		}

		public async Task UpdateCategoryParentAsync(CategoryParentCreate categoryParentCreate)
		{
			var categoryParentMap = _mapper.Map<CategoryParent>(categoryParentCreate);
			categoryParentMap.Slug = CreateSlug.Init_Slug(categoryParentCreate.Name);

			_context.CategoryParents!.Update(categoryParentMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCategoryParentAsync(int id)
		{
			var categoryParent = await _context.CategoryParents!.FirstOrDefaultAsync(c => c.Id == id);
			_context.CategoryParents!.Remove(categoryParent);
			await _context.SaveChangesAsync();
		}
	}
}
