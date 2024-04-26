using MediNet_BE.Dto.Payments.Momo;
using MediNet_BE.Dto.Payments.PayPal;
using MediNet_BE.Dto.Payments.VNPay;
using MediNet_BE.Services.Momo;
using MediNet_BE.Services.PayPal;
using MediNet_BE.Services.VNPay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediNet_BE.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class PaymentsController : ControllerBase
	{
		private IMomoService _momoService;
		private IVnPay _vnPay;
		private readonly IPayPalService _payPalService;

		public PaymentsController(IMomoService momoService, IVnPay vnPay, IPayPalService payPalService)
		{
			_momoService = momoService;
			_vnPay = vnPay;
			_payPalService = payPalService;
		}

		/// <summary>
		/// Create Momo
		/// </summary>
		/// <param name="model"></param>
		/// <remarks>
		/// "fullName": "Customer",
		/// "orderId": "54321",
		/// "orderInfo": "Order movie ticket",
		/// "amount": 12345
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
		[Route("Momo")]
		public async Task<IActionResult> Momo([FromBody] OrderInfoModel model)
		{
			var response = await _momoService.CreatePaymentAsync(model);
			return Ok(response.PayUrl);
		}

		//[HttpGet]
		//public IActionResult PaymentMomoCallBack()
		//{
		//    var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
		//    return Ok(response);
		//}

		/// <summary>
		/// Create VNPay
		/// </summary>
		/// <param name="vnPaymentRequestModel"></param>
		/// <remarks>
		/// "orderId": 54321,
		/// "fullName": "Customer",
		/// "description": "Order movie ticket",
		/// "amount": 12345,
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
		[Route("VnPay")]
		public IActionResult VnPay([FromBody] VnPaymentRequestModel vnPaymentRequestModel)
		{

			if (vnPaymentRequestModel == null)
				return BadRequest(ModelState);

			var paymentUrlString = _vnPay.CreatePaymentUrl(HttpContext, vnPaymentRequestModel);

			//var queryString = QueryHelpers.ParseQuery(new Uri(paymentUrlString).Query);
			//IQueryCollection createdVnpay = new QueryCollection(queryString);


			return Ok(paymentUrlString);
		}

		//[HttpGet]
		//public IActionResult PaymentVnPayCallBack()
		//{
		//    var response = _vnPay.PaymentExcute(Request.Query);
		//    if (response == null || response.VnPayResponseCode != "00")
		//    {
		//        return BadRequest("Thanh toan fail!");
		//    }
		//    return Ok(response);
		//}

		/// <summary>
		///  Create PayPal
		/// </summary>
		/// <param name="model"></param>
		/// <remarks>
		/// "orderType": "Sandbox",
		/// "amount": 999999,
		/// "orderDescription": "Order movie ticket",
		/// "name": "Customer"
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
		[Route("PayPal")]
		public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentInformationModel model)
		{
			var url = await _payPalService.CreatePaymentUrl(model, HttpContext);

			return Ok(url);
		}



	}
}
