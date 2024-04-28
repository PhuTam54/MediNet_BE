using MailKit.Net.Smtp;
using MailKit.Security;
using MediNet_BE.Dto.Mails;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;

namespace MediNet_BE.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(SendMailRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + $"\\wwwroot\\mails\\{request.Url}.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail)
                .Replace("verify", $"https://localhost:3000/Acccount/Verify?email={request.ToEmail}")
                .Replace("forgotpwd", $"https://localhost:3000/Acccount/ResetPassword?email={request.ToEmail}") ;
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
			email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
        }
		
		
	}
}
