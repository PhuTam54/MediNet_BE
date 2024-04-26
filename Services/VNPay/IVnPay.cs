
using MediNet_BE.Dto.Payments.VNPay;

namespace MediNet_BE.Services.VNPay
{
    public interface IVnPay
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
