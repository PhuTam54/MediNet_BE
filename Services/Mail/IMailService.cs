using MediNet_BE.Dto.Mails;

namespace MediNet_BE.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(SendMailRequest request);
    }
}
