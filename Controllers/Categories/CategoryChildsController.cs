using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Dto.Users;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Models.Categories;
using MediNet_BE.Interfaces.Categories;

namespace MediNet_BE.Controllers.Categories
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryChildsController : ControllerBase
    {
        private readonly ICategoryChildRepo _categoryChildRepo;
        private readonly ICategoryRepo _categoryRepo;

        public CategoryChildsController(ICategoryChildRepo categoryChildRepo, ICategoryRepo categoryRepo)
        {
            _categoryChildRepo = categoryChildRepo;
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryChildDto>>> GetCategoryChilds()
        {
            return Ok(await _categoryChildRepo.GetAllCategoryChildAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<CategoryChildDto>> GetCategoryChildById(int id)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(id);
            return categoryChild == null ? NotFound() : Ok(categoryChild);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryChild>> CreateCategoryChild([FromBody] CategoryChildDto categoryChildCreate)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(categoryChildCreate.CategoryId);
            if (category == null)
                return NotFound("Category Not Found!");
            if (categoryChildCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCategoryChild = await _categoryChildRepo.AddCategoryChildAsync(categoryChildCreate);
            return newCategoryChild == null ? NotFound() : Ok(newCategoryChild);
        }

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateCategoryChild([FromQuery] int id, [FromBody] CategoryChildDto updatedCategoryChild)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(id);
            var category = await _categoryRepo.GetCategoryByIdAsync(updatedCategoryChild.CategoryId);
            if (categoryChild == null)
                return NotFound();
            if (category == null)
                return NotFound("Category Not Found!");
            if (updatedCategoryChild == null)
                return BadRequest(ModelState);
            if (id != updatedCategoryChild.Id)
                return BadRequest();

            await _categoryChildRepo.UpdateCategoryChildAsync(updatedCategoryChild);

            return Ok("Update Successfully!");
        }

        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteCategoryChild([FromQuery] int id)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(id);
            if (categoryChild == null)
            {
                return NotFound();
            }
            await _categoryChildRepo.DeleteCategoryChildAsync(id);
            return Ok("Delete Successfully!");
        }
    }
}
