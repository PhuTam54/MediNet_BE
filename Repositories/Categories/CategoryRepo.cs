using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Categories
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

        public async Task<List<CategoryDto>> GetAllCategoryAsync()
        {
            var categories = await _context.Categories!
                .Include(cc => cc.CategoryChilds)
                .ToListAsync();
			
            var categoriesMap = _mapper.Map<List<CategoryDto>>(categories);

			return categoriesMap;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories!
                .Include(cc => cc.CategoryChilds)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
			var categoryMap = _mapper.Map<CategoryDto>(category);

			return categoryMap;
        }

        public async Task<Category> AddCategoryAsync(CategoryCreate categoryCreate)
        {
            var categoryParent = await _context.CategoryParents!.FirstOrDefaultAsync(cp => cp.Id == categoryCreate.CategoryParentId);
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            categoryMap.Slug = CreateSlug.Init_Slug(categoryCreate.Name);
            categoryMap.CategoryParent = categoryParent;

            _context.Categories!.Add(categoryMap);
            await _context.SaveChangesAsync();
            return categoryMap;
        }

        public async Task UpdateCategoryAsync(CategoryCreate categoryCreate)
        {
			var categoryParent = await _context.CategoryParents!.FirstOrDefaultAsync(cp => cp.Id == categoryCreate.CategoryParentId);
			var categoryMap = _mapper.Map<Category>(categoryCreate);
			categoryMap.Slug = CreateSlug.Init_Slug(categoryCreate.Name);
			categoryMap.CategoryParent = categoryParent;

			_context.Categories!.Update(categoryMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories!.SingleOrDefaultAsync(c => c.Id == id);

			_context.Categories!.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}
