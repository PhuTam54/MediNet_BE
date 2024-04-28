using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models;
using MediNet_BE.Interfaces;
using MediNet_BE.Repositories;
using MediNet_BE.Dto;

namespace MediNet_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
		private readonly ICategoryRepo _categoryRepo;

		public CategoriesController(ICategoryRepo categoryRepo)
        {
			_categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
			return Ok(await _categoryRepo.GetAllCategoryAsync());
		}

		[HttpGet]
        [Route("id")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
			var category = await _categoryRepo.GetCategoryByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

		[HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCategory = await _categoryRepo.AddCategoryAsync(categoryCreate);
            return newCategory == null ? NotFound() : Ok(newCategory);
        }

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCategory([FromQuery] int id, [FromBody] CategoryDto updatedCategory)
		{
			var category = await _categoryRepo.GetCategoryByIdAsync(id);
			if (category == null)
				return NotFound();
			if (updatedCategory == null)
				return BadRequest(ModelState);
			if (id != updatedCategory.Id)
				return BadRequest();

			await _categoryRepo.UpdateCategoryAsync(updatedCategory);

			return Ok("Update Successfully!");
		}

		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteCategory(int id)
        {
			var category = await _categoryRepo.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			await _categoryRepo.DeleteCategoryAsync(category);
			return Ok("Delete Successfully!");
		}

    }
}
