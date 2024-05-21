using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IFeedbackRepo
    {
        public Task<List<FeedbackDto>> GetAllFeedbackAsync();
        public Task<FeedbackDto> GetFeedbackByIdAsync(int id);
        public Task<List<FeedbackDto>> GetFeedbacksByProductIdAsync(int productId);
		public Task<List<FeedbackDto>> GetFeedbacksByCustomerIdAsync(int customerId);
		public Task<Feedback> AddFeedbackAsync(FeedbackCreate feedbackCreate);
        public Task UpdateFeedbackAsync(FeedbackCreate feedbackCreate);
        public Task DeleteFeedbackAsync(int id);
    }
}
