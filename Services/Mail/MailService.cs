using MailKit.Net.Smtp;
using MailKit.Security;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Orders;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

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
                .Replace("verify", $"https://localhost:3000/Acccount/verify?email={request.ToEmail}")
				.Replace("forgotpwd", $"http://localhost:3000/resetpassword?email={request.ToEmail}");
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

		public async Task SendOrderEmailAsync(OrderMailRequest request)
		{
			string FilePath = Directory.GetCurrentDirectory() + $"\\wwwroot\\mails\\{request.Url}.html";
			StreamReader str = new StreamReader(FilePath);
			string MailText = str.ReadToEnd();
			var dataList = ConvertObjectToHTMLList(request.Data);
			str.Close();
			MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail)
				.Replace("{DataList}", dataList)
				.Replace("[Name]", $"{request.UserName}")
				.Replace("[Email]", $"{request.ToEmail}")
				.Replace("[Phone]", $"{request.Data.Tel}")
				.Replace("[Address]", $"{request.Data.Address}");
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

		public string ConvertObjectToHTMLList(OrderDto orderDto)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var orderProduct in orderDto.OrderProducts)
			{
				sb.Append("<tr>");
				sb.Append($"<td style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>{orderProduct.Product.Name}</td>");
				sb.Append($"<td style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>{orderProduct.Quantity}</td>");
				sb.Append($"<td style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>{orderProduct.Product.Price}</td>");
				sb.Append($"<td style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>{orderProduct.Product.Price * orderProduct.Quantity}</td>");
				sb.Append("</tr>");
			}
			sb.Append("<tr>");
			sb.Append($"<td colspan='5' style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>GrandTotal</td>");
			sb.Append($"<td style='padding:0.75rem;vertical-align:top;border-top:1px solid #dee2e6;'>{orderDto.TotalAmount}.00</td>");
			sb.Append("</tr>");

			return sb.ToString();
		}

		
	}
}
