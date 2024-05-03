using System.ComponentModel.DataAnnotations;
using MediNet_BE.Dto.Users;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        [Required]
        public int Vote { get; set; }
        public string ImagesFeedback { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public IFormFile[]? ImagesFeedbackFile { get; set; }
    }
}
