using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces.Products;
using MediNet_BE.Models.Clinics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediNet_BE.Controllers.Clinics
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class StockOutsController : ControllerBase
	{
		private readonly IStockOutRepo _stockOutRepo;
		private readonly IProductRepo _productRepo;
		private readonly IClinicRepo _clinicRepo;

		public StockOutsController(IStockOutRepo stockOutRepo, IProductRepo productRepo, IClinicRepo clinicRepo)
		{
			_stockOutRepo = stockOutRepo;
			_productRepo = productRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<StockOutDto>>> GetCategories()
		{
			return Ok(await _stockOutRepo.GetAllStockOutAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<StockOutDto>> GetStockOutById(int id)
		{
			var stockOut = await _stockOutRepo.GetStockOutByIdAsync(id);
			return stockOut == null ? NotFound() : Ok(stockOut);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<StockOut>> CreateStockOut([FromBody] StockOutCreate stockOutCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(stockOutCreate.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(stockOutCreate.ClinicId);
			var stockOut = await _stockOutRepo.GetStockOutByProductIdAndClinicIdAsync(stockOutCreate.ProductId, stockOutCreate.ClinicId);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}

			if (stockOutCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newStockOut = await _stockOutRepo.AddStockOutAsync(stockOutCreate);
			return newStockOut == null ? NotFound() : Ok(newStockOut);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateStockOut([FromQuery] int id, [FromBody] StockOutCreate updatedStockOut)
		{
			var product = await _productRepo.GetProductByIdAsync(updatedStockOut.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedStockOut.ClinicId);
			var stockOut = await _stockOutRepo.GetStockOutByIdAsync(id);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}
			if (stockOut == null)
				return NotFound();
			if (updatedStockOut == null)
				return BadRequest(ModelState);
			if (id != updatedStockOut.Id)
				return BadRequest();

			await _stockOutRepo.UpdateStockOutAsync(updatedStockOut);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteStockOut([FromQuery] int id)
		{
			var stockOut = await _stockOutRepo.GetStockOutByIdAsync(id);
			if (stockOut == null)
			{
				return NotFound();
			}
			await _stockOutRepo.DeleteStockOutAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
