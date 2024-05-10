using System.ComponentModel.DataAnnotations;

namespace MediNet_BE.DtoCreate.Orders.OrderProducts
{
    public class FeedbackCreate
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
