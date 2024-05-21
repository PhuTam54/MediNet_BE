using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediNet_BE.Controllers.Employees.Blogs
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BlogCommentsController : ControllerBase
	{
		private readonly IBlogCommentRepo _blogCommentRepo;
		private readonly IBlogRepo _blogRepo;
		private readonly IUserRepo<Customer, CustomerDto, CustomerCreate> _customerRepo;

		public BlogCommentsController(IBlogCommentRepo blogCommentRepo, IBlogRepo blogRepo, IUserRepo<Customer, CustomerDto, CustomerCreate> customerRepo)
		{
			_blogCommentRepo = blogCommentRepo;
			_blogRepo = blogRepo;
			_customerRepo = customerRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BlogCommentDto>>> GetBlogComments()
		{
			return Ok(await _blogCommentRepo.GetAllBlogCommentAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<BlogCommentDto>> GetBlogCommentById([FromQuery] int id)
		{
			var blogComment = await _blogCommentRepo.GetBlogCommentByIdAsync(id);
			return blogComment == null ? NotFound() : Ok(blogComment);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
		[HttpPost]
		public async Task<ActionResult<BlogComment>> CreateBlogComment([FromBody] BlogCommentCreate blogCommentCreate)
		{
			var blog = await _blogRepo.GetBlogByIdAsync(blogCommentCreate.BlogId);
			var customer = await _customerRepo.GetUserByIdAsync(blogCommentCreate.CustomerId);
			if(blogCommentCreate.Parent_Id != 0)
			{
				var commentParent = await _blogCommentRepo.GetBlogCommentByIdAsync(blogCommentCreate.Parent_Id);
				if(commentParent == null)
					return NotFound();
			}
			if(customer == null || blog == null)
			{
				return NotFound();
			}
			if (blogCommentCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newBlogComment = await _blogCommentRepo.AddBlogCommentAsync(blogCommentCreate);
			return newBlogComment == null ? NotFound() : Ok(newBlogComment);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateBlogComment([FromQuery] int id, [FromBody] BlogCommentCreate updatedBlogComment)
		{
			var blogComment = await _blogCommentRepo.GetBlogCommentByIdAsync(id);
			var blog = await _blogRepo.GetBlogByIdAsync(updatedBlogComment.BlogId);
			var customer = await _customerRepo.GetUserByIdAsync(updatedBlogComment.CustomerId);
			if (customer == null || blog == null || blogComment == null)
			{
				return NotFound();
			}
			if (updatedBlogComment == null)
				return BadRequest(ModelState);
			if (id != updatedBlogComment.Id)
				return BadRequest();

			await _blogCommentRepo.UpdateBlogCommentAsync(updatedBlogComment);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Customer")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteBlogComment([FromQuery] int id)
		{
			var blogComment = await _blogCommentRepo.GetBlogCommentByIdAsync(id);
			if (blogComment == null)
			{
				return NotFound();
			}
			await _blogCommentRepo.DeleteBlogCommentAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
