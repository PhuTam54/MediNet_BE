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
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
		private readonly IOrderRepo _orderRepo;
		private readonly ICartRepo _cartRepo;
		private readonly IUserRepo<Customer, CustomerDto> _customerRepo;

		public OrdersController(IOrderRepo orderRepo, ICartRepo cartRepo, IUserRepo<Customer, CustomerDto> customerRepo)
		{
			_orderRepo = orderRepo;
			_cartRepo = cartRepo;
			_customerRepo = customerRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return Ok(await _orderRepo.GetAllOrderAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<Order>> GetOrderById(int id)
		{
			var order = await _orderRepo.GetOrderByIdAsync(id);
			return order == null ? NotFound() : Ok(order);
		}

		/// <summary>
		/// Create Order
		/// </summary>
		/// <param name="orderCreate"></param>
		/// <remarks>
		/// "name": "Tony",
		/// "email": "tony@gmail.com",
		/// "tel": "123534654358",
		/// "address": "123A - New York",
		/// "shipping_method": "string",
		/// "payment_method": "string",
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderCreate)
		{
			var user = await _customerRepo.GetUserByIdAsync(orderCreate.UserId);
			if (user == null)
				return NotFound("User Not Found!");
			foreach (var cartId in orderCreate.CartIds)
			{
				var cart = await _cartRepo.GetCartByIdAsync(cartId);
				if (cart == null)
				{
					return NotFound("Cart Not Found");
				}
			}
				if (orderCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newOrder = await _orderRepo.AddOrderAsync(orderCreate);
			return newOrder == null ? NotFound() : Ok(newOrder);
		}

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateOrder([FromQuery] int id, [FromBody] OrderDto updatedOrder)
		{
			var order = await _orderRepo.GetOrderByIdAsync(id);
			
			if (order == null)
				return NotFound();
			if (updatedOrder == null)
				return BadRequest(ModelState);
			if (id != updatedOrder.Id)
				return BadRequest();

			await _orderRepo.UpdateOrderAsync(updatedOrder);

			return Ok("Update Successfully!");
		}

		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var order = await _orderRepo.GetOrderByIdAsync(id);
			if (order == null)
			{
				return NotFound();
			}
			await _orderRepo.DeleteOrderAsync(order);
			return Ok("Delete Successfully!");
		}
	}
}
