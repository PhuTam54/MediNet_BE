using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Models.Categories;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Repositories.Orders;
using MediNet_BE.Repositories.Clinics;

namespace MediNet_BE.Controllers.Clinics
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SuppliesController : ControllerBase
    {
		private readonly ISupplyRepo _supplyRepo;
		private readonly IProductRepo _productRepo;
		private readonly IClinicRepo _clinicRepo;

		public SuppliesController(ISupplyRepo supplyRepo, IProductRepo productRepo, IClinicRepo clinicRepo)
		{
			_supplyRepo = supplyRepo;
			_productRepo = productRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Supply>>> GetCategories()
		{
			return Ok(await _supplyRepo.GetAllSupplyAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<Supply>> GetSupplyById(int id)
		{
			var supply = await _supplyRepo.GetSupplyByIdAsync(id);
			return supply == null ? NotFound() : Ok(supply);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<Supply>> CreateSupply([FromBody] SupplyDto supplyCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(supplyCreate.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(supplyCreate.ClinicId);
			var supply = await _supplyRepo.GetSupplyByProductIdAndClinicIdAsync(supplyCreate.ProductId, supplyCreate.ClinicId);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}

			if(supply != null)
			{
				return BadRequest("Supply already exists!");
			}

			if (supplyCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newSupply = await _supplyRepo.AddSupplyAsync(supplyCreate);
			return newSupply == null ? NotFound() : Ok(newSupply);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateSupply([FromQuery] int id, [FromBody] SupplyDto updatedSupply)
		{
			var product = await _productRepo.GetProductByIdAsync(updatedSupply.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedSupply.ClinicId);
			var supply = await _supplyRepo.GetSupplyByIdAsync(id);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}
			if (supply == null)
				return NotFound();
			if (updatedSupply == null)
				return BadRequest(ModelState);
			if (id != updatedSupply.Id)
				return BadRequest();

			await _supplyRepo.UpdateSupplyAsync(updatedSupply);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteSupply([FromQuery] int id)
		{
			var supply = await _supplyRepo.GetSupplyByIdAsync(id);
			if (supply == null)
			{
				return NotFound();
			}
			await _supplyRepo.DeleteSupplyAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
