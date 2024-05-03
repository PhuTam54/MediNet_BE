using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
{
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly MediNetContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepo(MediNetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Feedback>> GetAllFeedbackAsync()
        {
            var categories = await _context.Feedbacks!
                .Include(c => c.Customer)
                .Include(p => p.Product)
                .ToListAsync();

			return categories;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _context.Feedbacks!
								.Include(c => c.Customer)
								.Include(p => p.Product)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

			return feedback;
        }

        public async Task<Feedback> GetFeedbackByProductIdAsync(int productId)
        {
            var feedback = await _context.Feedbacks!
				.Include(c => c.Customer)
                .Include(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Product.Id == productId);

			return feedback;
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedbackMap = _mapper.Map<Feedback>(feedbackDto);

            _context.Feedbacks!.Add(feedbackMap);
            await _context.SaveChangesAsync();
            return feedbackMap;
        }

        public async Task UpdateFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedbackMap = _mapper.Map<Feedback>(feedbackDto);

            _context.Feedbacks!.Update(feedbackMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int id)
        {
			var feedback = await _context.Feedbacks!.FirstOrDefaultAsync(c => c.Id == id);

			_context.Feedbacks!.Remove(feedback);
            await _context.SaveChangesAsync();
        }


    }
}
