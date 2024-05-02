using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Interfaces;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;
using MediNet_BE.Helpers;
using Microsoft.CodeAnalysis;
using MediNet_BE.Dto.Payments.VNPay;
using MediNet_BE.Services.VNPay;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Services;
using Microsoft.Extensions.Primitives;
using MediNet_BE.Services.Momo;
using MediNet_BE.Services.PayPal;
using MediNet_BE.Dto.Payments.Momo;
using MediNet_BE.Dto.Payments.PayPal;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Models;
using MediNet_BE.Repositories.Orders;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IUserRepo<Customer, CustomerDto> _customerRepo;
        private readonly IVnPayService _vnPayService;
        private readonly MediNetContext _mediNetContext;
        private readonly IMailService _mailService;
        private readonly IMomoService _momoService;
        private readonly IPayPalService _payPalService;
		private readonly ICartRepo _cartRepo;

		public OrdersController(IOrderRepo orderRepo, IUserRepo<Customer, CustomerDto> customerRepo,
            IVnPayService vnPayService, MediNetContext mediNetContext, IMailService mailService,
            IMomoService momoService, IPayPalService payPalService, ICartRepo cartRepo)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _vnPayService = vnPayService;
            _mediNetContext = mediNetContext;
            _mailService = mailService;
            _momoService = momoService;
            _payPalService = payPalService;
			_cartRepo = cartRepo;

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

        [HttpGet]
        [Route("userId")]
        public async Task<ActionResult<OrderDto>> GetOrderByUserId(int userId)
        {
            var orderDto = await _orderRepo.GetOrderByUserIdAsync(userId);
            return orderDto == null ? NotFound() : Ok(orderDto);
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

        //[Authorize]
        //[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderCreate)
        {
            var user = await _customerRepo.GetUserByIdAsync(orderCreate.CustomerId);
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
            if (newOrder == null)
            {
                return NotFound();
            }

            if (newOrder.Payment_method == "Paypal")
            {
				var paypalModel = new PaymentInformationModel
                {
                    OrderId = newOrder.OrderCode,
                    Name = newOrder.Name,
                    OrderDescription = newOrder.Description,
                    Amount = (double)newOrder.TotalAmount
                };
                var response = await _payPalService.CreatePaymentUrl(paypalModel, HttpContext);
                return Ok(response);
            }
            if (newOrder.Payment_method == "VnPay")
            {
				
				var vnPayModel = new VnPaymentRequestModel
                {
					OrderId = newOrder.OrderCode,
					Amount = (double)newOrder.TotalAmount,
                    CreatedDate = DateTime.Now,
                    Description = $"{newOrder.Name} {newOrder.Tel}",
                    FullName = newOrder.Name,
				};
                return Ok(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            if (newOrder.Payment_method == "Momo")
            {
                var momoModel = new OrderInfoModel
                {
                    OrderId = newOrder.OrderCode,
                    Amount = (double)(newOrder.TotalAmount * 1000),
                    OrderInfo = newOrder.Address,
                    FullName = newOrder.Name
                };
                var response = await _momoService.CreatePaymentAsync(momoModel);
                return Ok(response.PayUrl);
            }

            var data = new SendMailRequest
            {
                ToEmail = newOrder.Email,
                UserName = newOrder.Name,
                Url = "thankyou",
                Subject = "Thank you for your order!"
            };
            await _mailService.SendEmailAsync(data);

            return Ok(newOrder);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Customer")]
        [NonAction]
        public async Task<Order> UpdateOrder(string orderCode)
        {
            var updatedOrder = await _mediNetContext.Orders.FirstOrDefaultAsync(m => m.OrderCode == orderCode);
            if (updatedOrder != null)
            {
                updatedOrder.Is_paid = true;
                updatedOrder.Status = OrderStatus.CONFIRMED;
                _mediNetContext.Update(updatedOrder);
                await _mediNetContext.SaveChangesAsync();

                var data = new SendMailRequest
                {
                    ToEmail = updatedOrder.Email,
                    UserName = updatedOrder.Name,
                    Url = "ThankYou",
                    Subject = "Thank you for your order!"
                };
                await _mailService.SendEmailAsync(data);
            }
            return updatedOrder;
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteOrder([FromQuery] int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            await _orderRepo.DeleteOrderAsync(id);
            return Ok("Delete Successfully!");
        }

        [HttpPut]
        [Route("VnPay")]
        public async Task<IActionResult> PaymentCallBackVnPay([FromQuery] Dictionary<string, string> data)
        {
            var queryCollection = new QueryCollection((Dictionary<string, StringValues>)data.Select(kv => new KeyValuePair<string, StringValues>(kv.Key, new StringValues(kv.Value))));
            var response = _vnPayService.PaymentExcute(queryCollection);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest($"Lỗi thanh toán vnpay: {response.VnPayResponseCode}");
            }
            var orderUpdate = await UpdateOrder(response.OrderId);
            return orderUpdate == null ? NotFound() : Ok(orderUpdate);
        }

        [HttpPut]
        [Route("Momo")]
        public async Task<IActionResult> PaymentCallBackMomo([FromQuery] Dictionary<string, string> data)
        {
            var queryCollection = new QueryCollection((Dictionary<string, StringValues>)data.Select(kv => new KeyValuePair<string, StringValues>(kv.Key, new StringValues(kv.Value))));
            var response = _momoService.PaymentExecuteAsync(queryCollection);

            var orderUpdate = await UpdateOrder(response.OrderId);
            return orderUpdate == null ? NotFound() : Ok(orderUpdate);
        }

        [HttpPut]
        [Route("Paypal")]
        public async Task<IActionResult> PaymentCallBackPaypal([FromQuery] Dictionary<string, string> data)
        {
            var queryCollection = new QueryCollection((Dictionary<string, StringValues>)data.Select(kv => new KeyValuePair<string, StringValues>(kv.Key, new StringValues(kv.Value))));
            var response = _payPalService.PaymentExecute(queryCollection);

            var orderUpdate = await UpdateOrder(response.OrderId);
            return orderUpdate == null ? NotFound() : Ok(orderUpdate);
        }

    }
}
