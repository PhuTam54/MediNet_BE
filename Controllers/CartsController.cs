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
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;
using MediNet_BE.Repositories;
using Microsoft.CodeAnalysis;

namespace MediNet_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
		private readonly ICartRepo _cartRepo;
		private readonly IUserRepo<Customer, CustomerDto> _customerRepo;
		private readonly IProductRepo _productRepo;

		public CartsController(ICartRepo cartRepo, IUserRepo<Customer, CustomerDto> customerRepo, IProductRepo productRepo)
		{
			_cartRepo = cartRepo;
			_customerRepo = customerRepo;
			_productRepo = productRepo;
		}

		[HttpGet]
		[Route("userid")]
		public async Task<ActionResult<IEnumerable<Cart>>> GetCartsByUserId(int userid)
		{
			var user = await _customerRepo.GetUserByIdAsync(userid);
			if (user == null)
				return NotFound("Customer Not Found!");
			var carts = await _cartRepo.GetCartsByUserIdAsync(userid);
			return Ok(carts);
		}

		[HttpPost]
		public async Task<ActionResult<Cart>> AddToCart([FromBody] CartDto cartCreate)
		{
			var user = await _customerRepo.GetUserByIdAsync(cartCreate.UserID);
			var product = await _productRepo.GetProductByIdAsync(cartCreate.ProductID);
			if(user == null || product == null)
				return NotFound();

			if (cartCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newCart = await _cartRepo.AddCartAsync(cartCreate);
			return newCart == null ? NotFound() : Ok(newCart);
		}

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCart([FromQuery] int id, [FromBody] CartDto updatedCart)
		{
			var user = await _customerRepo.GetUserByIdAsync(updatedCart.UserID);
			var product = await _productRepo.GetProductByIdAsync(updatedCart.ProductID);
			var cart = await _cartRepo.GetCartByIdAsync(id);

			if (user == null || product == null)
				return NotFound();
			if (cart == null)
				return NotFound();
			if (updatedCart == null)
				return BadRequest(ModelState);
			if (id != updatedCart.Id)
				return BadRequest();

			await _cartRepo.UpdateCartAsync(updatedCart);

			return Ok("Update Successfully!");
		}

		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteCart(int id)
		{
			var cart = await _cartRepo.GetCartByIdAsync(id);
			if (cart == null)
			{
				return NotFound();
			}
			await _cartRepo.DeleteCartAsync(cart);
			return Ok("Delete Successfully!");
		}
	}
}
