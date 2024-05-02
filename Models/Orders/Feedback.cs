using MediNet_BE.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Models.Orders
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Vote { get; set; }
        public string ImagesFeedback { get; set; }
		[NotMapped]
		public List<string> ImagesSrc { get; set; } = [];
		public string Description { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
