using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Models.Products;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Dto.Products;
using MediNet_BE.Interfaces.Products;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Services.Image;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Controllers.Products
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailRepo _productDetailRepo;
        private readonly IProductRepo _productRepo;
		private readonly IFileService _fileService;

		public ProductDetailsController(IProductDetailRepo productDetailRepo, IProductRepo productRepo, IFileService fileService)
        {
            _productDetailRepo = productDetailRepo;
            _productRepo = productRepo;
			_fileService = fileService;

		}

		[NonAction]
		public List<string> GetImagesPath(string path)
		{
			var imagesPath = new List<string>();
			string[] picturePaths = path.Split(';', StringSplitOptions.RemoveEmptyEntries);
			foreach (string picturePath in picturePaths)
			{
				var imageLink = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, picturePath);
				imagesPath.Add(imageLink);
			}
			return imagesPath;
		}

		[HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetailDto>>> GetProductDetails()
        {
            var productDetails = await _productDetailRepo.GetAllProductDetailAsync();
			foreach (var productDetail in productDetails)
			{
				productDetail.ImagesSrc.AddRange(GetImagesPath(productDetail.ImagesProductDetail));
			}
			return Ok(productDetails);
        }

        [HttpGet]
        [Route("productId")]
        public async Task<ActionResult<IEnumerable<ProductDetailDto>>> GetProductDetailsByProductId(int productId)
        {
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound("Product Not Found!");

			var productDetails = await _productDetailRepo.GetProductDetailsByProductIdAsync(productId);
			foreach (var productDetail in productDetails)
			{
				productDetail.ImagesSrc.AddRange(GetImagesPath(productDetail.ImagesProductDetail));
			}
			return Ok(productDetails);
        }
        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<ProductDetailDto>> GetProductDetailById([FromQuery] int id)
        {
            var productDetail = await _productDetailRepo.GetProductDetailByIdAsync(id);
			if (productDetail == null)
			{
				return NotFound();
			}
			productDetail.ImagesSrc.AddRange(GetImagesPath(productDetail.ImagesProductDetail));

			return Ok(productDetail);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDetail>> CreateProductDetail([FromForm] ProductDetailCreate productDetailCreate)
        {
            var product = await _productRepo.GetProductByIdAsync(productDetailCreate.ProductId);
            if (product == null)
                return NotFound("Product Not Found!");
            if (productDetailCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (productDetailCreate.ImagesProductDetailFile != null)
			{
				var fileResult = _fileService.SaveImages(productDetailCreate.ImagesProductDetailFile, "images/products/productDetails/");
				if (fileResult.Item1 == 1)
				{
					productDetailCreate.ImagesProductDetail = fileResult.Item2;
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			var newProductDetail = await _productDetailRepo.AddProductDetailAsync(productDetailCreate);
            return newProductDetail == null ? NotFound() : Ok(newProductDetail);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateProductDetail([FromQuery] int id, [FromForm] ProductDetailCreate updatedProductDetail)
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

			if (updatedProductDetail.ImagesProductDetailFile != null)
			{
				var fileResult = _fileService.SaveImages(updatedProductDetail.ImagesProductDetailFile, "images/products/productDetails/");
				if (fileResult.Item1 == 1)
				{
					updatedProductDetail.ImagesProductDetail = fileResult.Item2;
					await _fileService.DeleteImages(productDetail.ImagesProductDetail);

				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

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
			await _fileService.DeleteImages(productDetail.ImagesProductDetail);

			return Ok("Delete Successfully!");
        }
    }
}
