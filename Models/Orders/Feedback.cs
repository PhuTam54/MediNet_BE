using MediNet_BE.Models.Users;

namespace MediNet_BE.Models.Orders
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Vote { get; set; }
        public string ImagesFeedback { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
