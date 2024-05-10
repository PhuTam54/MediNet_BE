using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Helpers;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
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

        public async Task<List<FeedbackDto>> GetAllFeedbackAsync()
        {
            var feedbacks = await _context.Feedbacks!
                .Include(c => c.Customer)
			.Include(p => p.Product)
                .ToListAsync();

			var feedbacksMap = _mapper.Map<List<FeedbackDto>>(feedbacks);

			return feedbacksMap;
        }

        public async Task<FeedbackDto> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _context.Feedbacks!
								.Include(c => c.Customer)
								.Include(p => p.Product)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

			var feedbackMap = _mapper.Map<FeedbackDto>(feedback);

			return feedbackMap;
        }

        public async Task<FeedbackDto> GetFeedbackByProductIdAsync(int productId)
        {
            var feedback = await _context.Feedbacks!
				.Include(c => c.Customer)
                .Include(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Product.Id == productId);

			var feedbackMap = _mapper.Map<FeedbackDto>(feedback);

			return feedbackMap;
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackCreate feedbackCreate)
        {
            var feedbackMap = _mapper.Map<Feedback>(feedbackCreate);

            _context.Feedbacks!.Add(feedbackMap);
            await _context.SaveChangesAsync();
            return feedbackMap;
        }

        public async Task UpdateFeedbackAsync(FeedbackCreate feedbackCreate)
        {
            var feedbackMap = _mapper.Map<Feedback>(feedbackCreate);

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
