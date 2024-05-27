using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Interfaces.Products;

namespace MediNet_BE.Controllers.Clinics
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InStocksController : ControllerBase
    {
		private readonly IInStockRepo _inStockRepo;
		private readonly IProductRepo _productRepo;
		private readonly IClinicRepo _clinicRepo;

		public InStocksController(IInStockRepo inStockRepo, IProductRepo productRepo, IClinicRepo clinicRepo)
		{
			_inStockRepo = inStockRepo;
			_productRepo = productRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InStockDto>>> GetInStocks()
		{
			return Ok(await _inStockRepo.GetAllInStockAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<InStockDto>> GetInStockById(int id)
		{
			var inStock = await _inStockRepo.GetInStockByIdAsync(id);
			return inStock == null ? NotFound() : Ok(inStock);
		}

		[HttpGet]
		[Route("productIdAndClinicId")]
		public async Task<ActionResult<InStockDto>> GetInStockByProductIdAndClinicId(int productId, int clinicId)
		{
			var product = await _productRepo.GetProductByIdAsync(productId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(clinicId);
			if(product == null && clinic == null)
			{
				return NotFound();
			}
			var inStock = await _inStockRepo.GetInStockByProductIdAndClinicIdAsync(productId, clinicId);
			return inStock == null ? NotFound() : Ok(inStock);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<InStock>> CreateInStock([FromBody] InStockCreate inStockCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(inStockCreate.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(inStockCreate.ClinicId);
			var inStock = await _inStockRepo.GetInStockByProductIdAndClinicIdAsync(inStockCreate.ProductId, inStockCreate.ClinicId);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}

			if(inStock != null)
			{
				return BadRequest("InStock already exists!");
			}

			if (inStockCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newInStock = await _inStockRepo.AddInStockAsync(inStockCreate);
			return newInStock == null ? NotFound() : Ok(newInStock);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateInStock([FromQuery] int id, [FromBody] InStockCreate updatedInStock)
		{
			var product = await _productRepo.GetProductByIdAsync(updatedInStock.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedInStock.ClinicId);
			var inStock = await _inStockRepo.GetInStockByIdAsync(id);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}
			if (inStock == null)
				return NotFound();
			if (updatedInStock == null)
				return BadRequest(ModelState);
			if (id != updatedInStock.Id)
				return BadRequest();

			await _inStockRepo.UpdateInStockAsync(updatedInStock);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteInStock([FromQuery] int id)
		{
			var inStock = await _inStockRepo.GetInStockByIdAsync(id);
			if (inStock == null)
			{
				return NotFound();
			}
			await _inStockRepo.DeleteInStockAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
