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
	public class StockInsController : ControllerBase
	{
		private readonly IStockInRepo _stockInRepo;
		private readonly IProductRepo _productRepo;
		private readonly IClinicRepo _clinicRepo;

		public StockInsController(IStockInRepo stockInRepo, IProductRepo productRepo, IClinicRepo clinicRepo)
		{
			_stockInRepo = stockInRepo;
			_productRepo = productRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<StockInDto>>> GetCategories()
		{
			return Ok(await _stockInRepo.GetAllStockInAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<StockInDto>> GetStockInById(int id)
		{
			var stockIn = await _stockInRepo.GetStockInByIdAsync(id);
			return stockIn == null ? NotFound() : Ok(stockIn);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<StockIn>> CreateStockIn([FromBody] StockInCreate stockInCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(stockInCreate.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(stockInCreate.ClinicId);
			var stockIn = await _stockInRepo.GetStockInByProductIdAndClinicIdAsync(stockInCreate.ProductId, stockInCreate.ClinicId);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}

			if (stockIn != null)
			{
				return BadRequest("StockIn already exists!");
			}

			if (stockInCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newStockIn = await _stockInRepo.AddStockInAsync(stockInCreate);
			return newStockIn == null ? NotFound() : Ok(newStockIn);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateStockIn([FromQuery] int id, [FromBody] StockInCreate updatedStockIn)
		{
			var product = await _productRepo.GetProductByIdAsync(updatedStockIn.ProductId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedStockIn.ClinicId);
			var stockIn = await _stockInRepo.GetStockInByIdAsync(id);

			if (product == null || clinic == null)
			{
				return NotFound("Product or Cinic Not Found!");
			}
			if (stockIn == null)
				return NotFound();
			if (updatedStockIn == null)
				return BadRequest(ModelState);
			if (id != updatedStockIn.Id)
				return BadRequest();

			await _stockInRepo.UpdateStockInAsync(updatedStockIn);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteStockIn([FromQuery] int id)
		{
			var stockIn = await _stockInRepo.GetStockInByIdAsync(id);
			if (stockIn == null)
			{
				return NotFound();
			}
			await _stockInRepo.DeleteStockInAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
