using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Orders;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Repositories.Orders;
using MediNet_BE.DtoCreate.Orders.OrderProducts;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
		private readonly IProductDetailRepo _productDetailRepo;
		private readonly IProductRepo _productRepo;

		public ProductDetailsController(IProductDetailRepo productDetailRepo, IProductRepo productRepo)
		{
			_productDetailRepo = productDetailRepo;
			_productRepo =	productRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDetailDto>>> GetProductDetails()
		{
			return Ok(await _productDetailRepo.GetAllProductDetailAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<ProductDetailDto>> GetProductDetail([FromQuery] int id)
		{
			var productDetail = await _productDetailRepo.GetProductDetailByIdAsync(id);
			return productDetail == null ? NotFound() : Ok(productDetail);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<ProductDetail>> CreateProductDetail([FromBody] ProductDetailCreate productDetailCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(productDetailCreate.ProductId);
			if (product == null)
				return NotFound("Product Not Found!");
			if (productDetailCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newProductDetail = await _productDetailRepo.AddProductDetailAsync(productDetailCreate);
			return newProductDetail == null ? NotFound() : Ok(newProductDetail);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateProductDetail([FromQuery] int id, [FromBody] ProductDetailCreate updatedProductDetail)
		{
			var product = await _productRepo.GetProductByIdAsync(updatedProductDetail.ProductId);
			var productDetail = await _productDetailRepo.GetProductDetailByIdAsync(id);

			if (product == null)
				return NotFound("Product Not Found!");
			if (productDetail == null)
				return NotFound();
			if (updatedProductDetail == null)
				return BadRequest(ModelState);
			if (id != updatedProductDetail.Id)
				return BadRequest();

			await _productDetailRepo.UpdateProductDetailAsync(updatedProductDetail);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteProductDetail([FromQuery] int id)
		{
			var productDetail = await _productDetailRepo.GetProductDetailByIdAsync(id);
			if (productDetail == null)
			{
				return NotFound();
			}
			await _productDetailRepo.DeleteProductDetailAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
