using MediNet_BE.Dto.Payments.Momo;

namespace MediNet_BE.Services.Momo;

public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
    MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
}