using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Services.Image;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Products;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Dto.Products;
using MediNet_BE.Interfaces.Products;

namespace MediNet_BE.Controllers.Products
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryChildRepo _categoryChildRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ICategoryParentRepo _categoryParentRepo;
        private readonly IClinicRepo _clinicRepo;
        private readonly IFileService _fileService;

        public ProductsController(IProductRepo productRepo, ICategoryChildRepo categoryChildRepo, ICategoryRepo categoryRepo, ICategoryParentRepo categoryParentRepo,
            IClinicRepo clinicRepo, IFileService fileService)
        {
            _productRepo = productRepo;
            _categoryChildRepo = categoryChildRepo;
            _categoryRepo = categoryRepo;
            _categoryParentRepo = categoryParentRepo;
            _clinicRepo = clinicRepo;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productRepo.GetAllProductAsync();
            foreach (var product in products)
            {
                product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("buyQty")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBuyQty()
        {
            var products = await _productRepo.GetProductsByBuyQtyAsync();
            foreach (var product in products)
            {
                product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);

            return Ok(product);
        }

        [HttpGet]
        [Route("categoryChildId")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryChildIdAsync(int categoryChildId)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(categoryChildId);

            if (categoryChild == null)
                return NotFound("Category Child Not Found!");
            var products = await _productRepo.GetProductsByCategoryChildIdAsync(categoryChildId);
            foreach (var product in products)
            {
                product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("categoryId")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound("Category Not Found!");
            var products = await _productRepo.GetProductsByCategoryIdAsync(categoryId);
            foreach (var product in products)
            {
                product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("categoryParentId")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryParentIdAsync(int categoryParentId)
        {
            var categoryParent = await _categoryChildRepo.GetCategoryChildByIdAsync(categoryParentId);

            if (categoryParent == null)
                return NotFound("Category Parent Not Found!");
            var products = await _productRepo.GetProductsByCategoryParentIdAsync(categoryParentId);
            foreach (var product in products)
            {
                product.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, product.Image);
            }
            return Ok(products);
        }
        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productCreate"></param>
        /// /// <remarks>
        /// ManufacturerDate
        /// 2024-04-13T08:18:59.6300000
        /// </remarks>
        /// <returns></returns>

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreate productCreate)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(productCreate.CategoryChildId);

            if (categoryChild == null)
                return NotFound("Category Child Not Found!");

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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromForm] ProductCreate updatedProduct)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(updatedProduct.CategoryChildId);

            var product = await _productRepo.GetProductByIdAsync(id);

            if (categoryChild == null)
                return NotFound("Category Child Not Found!");

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
                    await _fileService.DeleteImage(product.Image);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            await _productRepo.UpdateProductAsync(updatedProduct);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepo.DeleteProductAsync(id);
            await _fileService.DeleteImage(product.Image);
            return Ok("Delete Successfully!");
        }
    }
}
