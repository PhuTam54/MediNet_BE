using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Dto.Mails
{
	public class OrderMailRequest
	{
		public string ToEmail { get; set; }
		public string UserName { get; set; }
		public string Url { get; set; }
		public string Subject { get; set; }
		public OrderDto Data { get; set; }
	}
}
