using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Interfaces;
using MediNet_BE.Services.Image;
using MediNet_BE.Models.Users;
using MediNet_BE.Dto.Users;
using MediNet_BE.Repositories;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Dto;
using NuGet.Packaging;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackRepo _feedbackRepo;
        private readonly IFileService _fileService;
        private readonly IUserRepo<Customer, CustomerDto> _customerRepo;
        private readonly IProductRepo _productRepo;

        public FeedbacksController(IFeedbackRepo feedbackRepo, IFileService fileService, IUserRepo<Customer, CustomerDto> customerRepo, IProductRepo productRepo)
        {
            _feedbackRepo = feedbackRepo;
            _fileService = fileService;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
        }

        [NonAction]
        public List<string> GetImagesPath(string path)
        {
            var imagesPath = new List<string>();
			string[] picturePaths = path.Split(';', StringSplitOptions.RemoveEmptyEntries);
			foreach (string picturePath in picturePaths)
			{
				var imageLink = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, picturePath);
				imagesPath.Add(imageLink);
			}
            return imagesPath;
		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetCategories()
        {
            var feedbacksDto = await _feedbackRepo.GetAllFeedbackAsync();
			foreach (var feedbackDto in feedbacksDto)
			{
                feedbackDto.ImagesSrc.AddRange(GetImagesPath(feedbackDto.ImagesFeedback));
			}
            
			return Ok(feedbacksDto);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackById(int id)
        {
            var feedbackDto = await _feedbackRepo.GetFeedbackByIdAsync(id);
			if (feedbackDto == null)
			{
				return NotFound();
			}
			feedbackDto.ImagesSrc.AddRange(GetImagesPath(feedbackDto.ImagesFeedback));

			return Ok(feedbackDto);
        }

        [HttpGet]
        [Route("productId")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackByProductId(int productId)
        {
            var feedbackDto = await _feedbackRepo.GetFeedbackByProductIdAsync(productId);
			if (feedbackDto == null)
			{
				return NotFound();
			}
			feedbackDto.ImagesSrc.AddRange(GetImagesPath(feedbackDto.ImagesFeedback));

			return Ok(feedbackDto);
        }

        [HttpPost]
        public async Task<ActionResult<Feedback>> CreateFeedback([FromForm] FeedbackDto feedbackCreate)
        {
            var customer = await _customerRepo.GetUserByIdAsync(feedbackCreate.CustomerId);
            var product = await _productRepo.GetProductByIdAsync(feedbackCreate.ProductId);
            if (customer == null || product == null)
            {
                return NotFound("Customer or Product Not Found!");
            }
            if (feedbackCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (feedbackCreate.ImagesFeedbackFile != null)
            {
                var fileResult = _fileService.SaveImages(feedbackCreate.ImagesFeedbackFile, "images/users/feedbacks/");
                if (fileResult.Item1 == 1)
                {
                    feedbackCreate.ImagesFeedback = fileResult.Item2;
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }
            var newFeedback = await _feedbackRepo.AddFeedbackAsync(feedbackCreate);
            return newFeedback == null ? NotFound() : Ok(newFeedback);
        }

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateFeedback([FromQuery] int id, [FromForm] FeedbackDto updatedFeedback)
        {
            var feedback = await _feedbackRepo.GetFeedbackByIdAsync(id);
            if (feedback == null)
                return NotFound();
            if (updatedFeedback == null)
                return BadRequest(ModelState);
            if (id != updatedFeedback.Id)
                return BadRequest();

            if (updatedFeedback.ImagesFeedbackFile != null)
            {
                var fileResult = _fileService.SaveImages(updatedFeedback.ImagesFeedbackFile, "images/users/feedbacks/");
                if (fileResult.Item1 == 1)
                {
                    updatedFeedback.ImagesFeedback = fileResult.Item2;
                    await _fileService.DeleteImages(feedback.ImagesFeedback);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            await _feedbackRepo.UpdateFeedbackAsync(updatedFeedback);

            return Ok("Update Successfully!");
        }

        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteFeedback([FromQuery] int id)
        {
            var feedback = await _feedbackRepo.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            await _feedbackRepo.DeleteFeedbackAsync(id);
            await _fileService.DeleteImages(feedback.ImagesFeedback);

            return Ok("Delete Successfully!");
        }
    }
}
