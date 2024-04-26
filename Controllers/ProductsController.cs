using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Repositories;
using MediNet_BE.Services.Image;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
		private readonly IProductRepo _productRepo;
		private readonly ICategoryChildRepo _categoryChildRepo;
		private readonly IClinicRepo _clinicRepo;
		private readonly IFileService _fileService;

		public ProductsController(IProductRepo productRepo,ICategoryChildRepo categoryChildRepo, IClinicRepo clinicRepo, IFileService fileService)
		{
			_productRepo = productRepo;
			_categoryChildRepo = categoryChildRepo;
			_clinicRepo = clinicRepo;
			_fileService = fileService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetCategories()
		{
			return Ok(await _productRepo.GetAllProductAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			var product = await _productRepo.GetProductByIdAsync(id);
			return product == null ? NotFound() : Ok(product);
		}

		/// <summary>
		/// Create Product
		/// </summary>
		/// <param name="productCreate"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductDto productCreate)
		{
			var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(productCreate.CategoryChildId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(productCreate.ClinicId);

			if (categoryChild == null)
				return NotFound("Category Child Not Found!");
			if (clinic == null)
				return NotFound("Clinic Not Found!");
			if (productCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (productCreate.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(productCreate.ImageFile, "images/products/");
				if (fileResult.Item1 == 1)
				{
					productCreate.Image = fileResult.Item2;
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			var newProduct = await _productRepo.AddProductAsync(productCreate);
			return newProduct == null ? NotFound() : Ok(newProduct);
		}

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromForm] ProductDto updatedProduct)
		{
			var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(updatedProduct.CategoryChildId);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedProduct.ClinicId);
			var product = await _productRepo.GetProductByIdAsync(id);

			if (categoryChild == null)
				return NotFound("Category Child Not Found!");
			if (clinic == null)
				return NotFound("Clinic Not Found!");
			if (product == null)
				return NotFound();
			if (updatedProduct == null)
				return BadRequest(ModelState);
			if (id != updatedProduct.Id)
				return BadRequest();

			if (updatedProduct.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(updatedProduct.ImageFile, "images/products/");
				if (fileResult.Item1 == 1)
				{
					updatedProduct.Image = fileResult.Item2;
					await _fileService.DeleteImage(product.Image, "images/products/");
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			await _productRepo.UpdateProductAsync(updatedProduct);

			return Ok("Update Successfully!");
		}

		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _productRepo.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			await _productRepo.DeleteProductAsync(product);
			await _fileService.DeleteImage(product.Image, "images/products/");
			return Ok("Delete Successfully!");
		}
	}
}
