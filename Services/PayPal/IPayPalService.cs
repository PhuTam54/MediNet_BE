
using MediNet_BE.Dto.Payments.PayPal;

namespace MediNet_BE.Services.PayPal;
public interface IPayPalService
{
    Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}