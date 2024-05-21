using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Models.Categories;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Dto.Categories;
using MediNet_BE.DtoCreate.Categories;

namespace MediNet_BE.Controllers.Categories
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryParentsController : ControllerBase
    {
		private readonly ICategoryParentRepo _categoryParentRepo;

		public CategoryParentsController(ICategoryParentRepo categoryParentRepo)
		{
			_categoryParentRepo = categoryParentRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryParentDto>>> GetCategoryParents()
		{
			return Ok(await _categoryParentRepo.GetAllCategoryParentAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<CategoryParentDto>> GetCategoryParentById([FromQuery] int id)
		{
			var categoryParent = await _categoryParentRepo.GetCategoryParentByIdAsync(id);
			return categoryParent == null ? NotFound() : Ok(categoryParent);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<CategoryParent>> CreateCategoryParent([FromBody] CategoryParentCreate categoryParentCreate)
		{
			if (categoryParentCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newCategoryParent = await _categoryParentRepo.AddCategoryParentAsync(categoryParentCreate);
			return newCategoryParent == null ? NotFound() : Ok(newCategoryParent);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCategoryParent([FromQuery] int id, [FromBody] CategoryParentCreate updatedCategoryParent)
		{
			var categoryParent = await _categoryParentRepo.GetCategoryParentByIdAsync(id);
			if (categoryParent == null)
				return NotFound();
			if (updatedCategoryParent == null)
				return BadRequest(ModelState);
			if (id != updatedCategoryParent.Id)
				return BadRequest();

			await _categoryParentRepo.UpdateCategoryParentAsync(updatedCategoryParent);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteCategoryParent([FromQuery] int id)
		{
			var categoryParent = await _categoryParentRepo.GetCategoryParentByIdAsync(id);
			if (categoryParent == null)
			{
				return NotFound();
			}
			await _categoryParentRepo.DeleteCategoryParentAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
