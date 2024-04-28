namespace MediNet_BE.Dto.Mails
{
	public class SendMailRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Subject { get; set; }
    }
}
