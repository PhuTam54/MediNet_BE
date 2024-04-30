using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Repositories;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Models.Categories;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MediNet_BE.Controllers.Categories
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
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            return Ok(await _categoryRepo.GetAllCategoryAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var categoryDto = await _categoryRepo.GetCategoryByIdAsync(id);
            return categoryDto == null ? NotFound() : Ok(categoryDto);
        }

        /// <summary>
        /// categoryCreate
        /// </summary>
        /// <param name="categoryCreate"></param>
        /// <remarks>
        /// {
        ///   "id": 0,
        ///   "name": "MedicalEquipment",
        ///   "categoryChilds": [
        ///   ]
        /// }
        /// </remarks>
        /// <returns></returns>

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteCategory([FromQuery] int id)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryRepo.DeleteCategoryAsync(id);
            return Ok("Delete Successfully!");
        }

    }
}
