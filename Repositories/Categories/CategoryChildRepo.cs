﻿using AutoMapper;
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
    public class CategoryChildRepo : ICategoryChildRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public CategoryChildRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryChildDto>> GetAllCategoryChildAsync()
        {
            var categoryChilds = await _context.CategoryChilds!
                .Include(c => c.Category)
			.Include(p => p.Products)
                .ToListAsync();
			var categoryChildsMap = _mapper.Map<List<CategoryChildDto>>(categoryChilds);

			return categoryChildsMap;
        }

        public async Task<CategoryChildDto> GetCategoryChildByIdAsync(int id)
        {
            var categoryChild = await _context.CategoryChilds!
                .Include(c => c.Category)
				.Include(p => p.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(cc => cc.Id == id);

			var categoryChildMap = _mapper.Map<CategoryChildDto>(categoryChild);

			return categoryChildMap;
        }

        public async Task<CategoryChild> AddCategoryChildAsync(CategoryChildCreate categoryChildCreate)
        {
            var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildCreate.CategoryId);
            var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildCreate);
            categoryChildMap.Slug = CreateSlug.Init_Slug(categoryChildCreate.Name);
            categoryChildMap.Category = catetory;

            _context.CategoryChilds!.Add(categoryChildMap);
            await _context.SaveChangesAsync();
            return categoryChildMap;
        }

        public async Task UpdateCategoryChildAsync(CategoryChildCreate categoryChildCreate)
        {
            var catetory = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == categoryChildCreate.CategoryId);
            var categoryChildMap = _mapper.Map<CategoryChild>(categoryChildCreate);
            categoryChildMap.Slug = CreateSlug.Init_Slug(categoryChildCreate.Name);
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
