using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Categories
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
            var categoryChilds = await _context.CategoryChilds!
                .Include(c => c.Category)
                .Include(p => p.Products)
                .ToListAsync();

			return categoryChilds;
        }

        public async Task<CategoryChild> GetCategoryChildByIdAsync(int id)
        {
            var categoryChild = await _context.CategoryChilds!
                .Include(c => c.Category)
				.Include(p => p.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(cc => cc.Id == id);

			return categoryChild;
        }

        public async Task<CategoryChild> AddCategoryChildAsync(CategoryChildDto categoryChildDto)
        {
            var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildDto.CategoryId);
            var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildDto);
            categoryChildMap.Slug = CreateSlug.Init_Slug(categoryChildDto.Name);
            categoryChildMap.Category = catetory;

            _context.CategoryChilds!.Add(categoryChildMap);
            await _context.SaveChangesAsync();
            return categoryChildMap;
        }

        public async Task UpdateCategoryChildAsync(CategoryChildDto categoryChildDto)
        {
            var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildDto.CategoryId);
            var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildDto);
            categoryChildMap.Slug = CreateSlug.Init_Slug(categoryChildDto.Name);
            categoryChildMap.Category = catetory;

            _context.CategoryChilds!.Update(categoryChildMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryChildAsync(int id)
        {
            var categoryChild = await _context.CategoryChilds!.FirstOrDefaultAsync(cc => cc.Id == id);

			_context.CategoryChilds!.Remove(categoryChild);
            await _context.SaveChangesAsync();
        }
    }
}
