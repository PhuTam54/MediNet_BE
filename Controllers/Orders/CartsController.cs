using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models;
using MediNet_BE.Interfaces;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;
using MediNet_BE.Repositories;
using Microsoft.CodeAnalysis;
using MediNet_BE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Repositories.Clinics;
using MediNet_BE.Dto.Orders.OrderProducts;

namespace MediNet_BE.Controllers.Orders
{

    [Route("api/v1/[controller]")]
    [ApiController]
	public class CartsController : ControllerBase
	{
		private readonly ICartRepo _cartRepo;
		private readonly IUserRepo<Customer, CustomerDto> _customerRepo;
		private readonly IProductRepo _productRepo;
		private readonly IClinicRepo _clinicRepo;

		public CartsController(ICartRepo cartRepo, IUserRepo<Customer, CustomerDto> customerRepo, IProductRepo productRepo, IClinicRepo clinicRepo)
		{
			_cartRepo = cartRepo;
			_customerRepo = customerRepo;
			_productRepo = productRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		[Route("userid")]
		public async Task<ActionResult<IEnumerable<Cart>>> GetCartsByUserId(int userid)
		{
			var user = await _customerRepo.GetUserByIdAsync(userid);
			if (user == null)
				return NotFound("Customer Not Found!");
			var carts = await _cartRepo.GetCartsByCustomerIdAsync(userid);
			return Ok(carts);
		}

		[HttpPost]
		public async Task<ActionResult<Cart>> AddToCart([FromBody] CartDto cartCreate)
		{
			var customer = await _customerRepo.GetUserByIdAsync(cartCreate.CustomerID);
			var product = await _productRepo.GetProductByIdAsync(cartCreate.ProductID);
			var clinic = await _clinicRepo.GetClinicByIdAsync(cartCreate.ClinicID);
			if (customer == null || product == null || clinic == null)
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
			var customer = await _customerRepo.GetUserByIdAsync(updatedCart.CustomerID);
			var product = await _productRepo.GetProductByIdAsync(updatedCart.ProductID);
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedCart.ClinicID);
			var cart = await _cartRepo.GetCartByIdAsync(id);

			if (customer == null || product == null || clinic == null)
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
