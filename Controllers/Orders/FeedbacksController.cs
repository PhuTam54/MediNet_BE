using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Interfaces;
using MediNet_BE.Services.Image;
using MediNet_BE.Models.Users;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.Interfaces.Products;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackRepo _feedbackRepo;
        private readonly IFileService _fileService;
        private readonly IUserRepo<Customer, CustomerDto, CustomerCreate> _customerRepo;
        private readonly IProductRepo _productRepo;

        public FeedbacksController(IFeedbackRepo feedbackRepo, IFileService fileService, IUserRepo<Customer, CustomerDto, CustomerCreate> customerRepo, IProductRepo productRepo)
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
                var imageLink = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, picturePath);
                imagesPath.Add(imageLink);
            }
            return imagesPath;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetCategories()
        {
            var feedbacks = await _feedbackRepo.GetAllFeedbackAsync();
            foreach (var feedback in feedbacks)
            {
                feedback.ImagesSrc.AddRange(GetImagesPath(feedback.ImagesFeedback));
            }

            return Ok(feedbacks);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackById(int id)
        {
            var feedback = await _feedbackRepo.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            feedback.ImagesSrc.AddRange(GetImagesPath(feedback.ImagesFeedback));

            return Ok(feedback);
        }

        [HttpGet]
        [Route("productId")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacksByProductId(int productId)
        {
			var feedbacks = await _feedbackRepo.GetFeedbacksByProductIdAsync(productId);

			if (feedbacks == null)
			{
				return NotFound();
			}

            foreach (var feedback in feedbacks)
            {
                feedback.ImagesSrc.AddRange(GetImagesPath(feedback.ImagesFeedback));
            }
            return Ok(feedbacks);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Customer")]
        [HttpPost]
        public async Task<ActionResult<Feedback>> CreateFeedback([FromForm] FeedbackCreate feedbackCreate)
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Customer")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateFeedback([FromQuery] int id, [FromForm] FeedbackCreate updatedFeedback)
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Customer")]
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
