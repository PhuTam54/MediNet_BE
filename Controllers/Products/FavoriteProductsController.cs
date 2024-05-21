using MediNet_BE.Dto.Products;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces;
using MediNet_BE.Interfaces.Products;
using MediNet_BE.Models.Products;
using MediNet_BE.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediNet_BE.Controllers.Products
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class FavoriteProductsController : ControllerBase
	{
		private readonly IFavoriteProductRepo _favoriteProductRepo;
		private readonly IProductRepo _productRepo;
		private readonly IUserRepo<Customer, CustomerDto, CustomerCreate> _customerRepo;

		public FavoriteProductsController(IFavoriteProductRepo favoriteProductRepo, IProductRepo productRepo, IUserRepo<Customer, CustomerDto, CustomerCreate> customerRepo)
		{
			_favoriteProductRepo = favoriteProductRepo;
			_productRepo = productRepo;
			_customerRepo = customerRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<FavoriteProductDto>>> GetFavoriteProducts()
		{
			return Ok(await _favoriteProductRepo.GetAllFavoriteProductAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<FavoriteProductDto>> GetFavoriteProductById([FromQuery] int id)
		{
			var favoriteProduct = await _favoriteProductRepo.GetFavoriteProductByIdAsync(id);
			return favoriteProduct == null ? NotFound() : Ok(favoriteProduct);
		}

		[HttpGet]
		[Route("customerId")]
		public async Task<ActionResult<IEnumerable<FavoriteProductDto>>> GetFavoriteProductsByCustomerId([FromQuery] int customerId)
		{
			var customer = await _customerRepo.GetUserByIdAsync(customerId);
			if(customer == null)
				return NotFound();
			return Ok(await _favoriteProductRepo.GetFavoriteProductsByCustomerIdAsync(customerId));
		}

		[HttpGet]
		[Route("productId")]
		public async Task<ActionResult<IEnumerable<FavoriteProductDto>>> GetFavoriteProductsByProductId([FromQuery] int productId)
		{
			var product = await _productRepo.GetProductByIdAsync(productId);
			if (product == null)
				return NotFound();
			return Ok(await _favoriteProductRepo.GetFavoriteProductsByProductIdAsync(productId));
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
		[HttpPost]
		public async Task<ActionResult<FavoriteProduct>> CreateFavoriteProduct([FromBody] FavoriteProductCreate favoriteProductCreate)
		{
			var product = await _productRepo.GetProductByIdAsync(favoriteProductCreate.ProductId);
			var customer = await _customerRepo.GetUserByIdAsync(favoriteProductCreate.CustomerId);
			if (customer == null || product == null)
			{
				return NotFound();
			}
			if (favoriteProductCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newFavoriteProduct = await _favoriteProductRepo.AddFavoriteProductAsync(favoriteProductCreate);
			return newFavoriteProduct == null ? NotFound() : Ok(newFavoriteProduct);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteFavoriteProduct([FromQuery] int id)
		{
			var favoriteProduct = await _favoriteProductRepo.GetFavoriteProductByIdAsync(id);
			if (favoriteProduct == null)
			{
				return NotFound();
			}
			await _favoriteProductRepo.DeleteFavoriteProductAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
