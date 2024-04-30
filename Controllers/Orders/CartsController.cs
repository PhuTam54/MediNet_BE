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
using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Controllers.Orders
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly MediNetContext _context;
        public CartsController(MediNetContext context)
        {
            _context = context;
        }
        public List<CartItem> MyCart => HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> getCartByUserId(int userId)
        {
            var carts = MyCart;
            var user = await _context.Customers.SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            var cartUser = carts.Where(c => c.UserID == userId).ToList();
            if (cartUser != null)
            {
                return Ok(cartUser);
            }
            return Ok("Cart is empty!");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int userId, int buy_qty)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.SingleOrDefault(c => c.ProductID == productId && c.UserID == userId);

            if (item == null)
            {
                var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
                var user = await _context.Customers.SingleOrDefaultAsync(u => u.Id == userId);
                if (product == null || user == null)
                {
                    return NotFound();
                }
                item = new CartItem
                {
                    ProductID = product.Id,
                    UserID = user.Id,
                    Name = product.Name,
                    Image = product.Image,
                    Price = product.Price,
                    Description = product.Description,
                    Qty = buy_qty
                };
                cart.Add(item);
            }
            else
            {
                item.Qty += buy_qty;
            }
            HttpContext.Session.Set("Cart", cart);
            return Ok(cart);
        }

        [HttpDelete]
        public IActionResult RemoveToCart(int? productId, int? userId)
        {
            if (productId == null)
            {
                return NotFound();
            }
            var cart = MyCart;
            if (cart != null)
            {
                var carToRemove = cart.FirstOrDefault(p => p.ProductID == productId && p.UserID == userId);
                if (carToRemove != null)
                {
                    cart.Remove(carToRemove);
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return Ok("Remove cart successfully!");
        }
    }
}
