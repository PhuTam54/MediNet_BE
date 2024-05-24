using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Dto.Employees;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Dto.Employees.Blogs;
using AutoMapper;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Services.Image;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.Models.Products;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Controllers.Employees.Blogs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepo _blogRepo;
        private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;
        private readonly IDiseaseRepo _diseaseRepo;
		private readonly IFileService _fileService;

		public BlogsController(IBlogRepo blogRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo, IDiseaseRepo diseaseRepo, IFileService fileService)
        {
            _blogRepo = blogRepo;
            _employeeRepo = employeeRepo;
            _diseaseRepo = diseaseRepo;
			_fileService = fileService;
		}

		[HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetBlogs()
        {
            var blogs = await _blogRepo.GetAllBlogAsync();
			foreach (var blog in blogs)
			{
				blog.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, blog.Image);
			}
			return Ok(blogs);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<BlogDto>> GetBlogById([FromQuery] int id)
        {
            var blog = await _blogRepo.GetBlogByIdAsync(id);
			if (blog == null)
			{
				return NotFound();
			}
			blog.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, blog.Image);

			return Ok(blog);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog([FromForm] BlogCreate blogCreate)
        {
            var doctorDto = await _employeeRepo.GetUserByIdAsync(blogCreate.EmployeeId);
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(blogCreate.DiseaseId);

            if (doctorDto == null || disease == null)
            {
                return NotFound();
            }
            if (blogCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (blogCreate.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(blogCreate.ImageFile, "images/blogs/");
				if (fileResult.Item1 == 1)
				{
					blogCreate.Image = fileResult.Item2;
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			var newBlog = await _blogRepo.AddBlogAsync(blogCreate);
            return newBlog == null ? NotFound() : Ok(newBlog);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateBlog([FromQuery] int id, [FromForm] BlogCreate updatedBlog)
        {
            var doctorDto = await _employeeRepo.GetUserByIdAsync(updatedBlog.EmployeeId);
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(updatedBlog.DiseaseId);
            var blog = await _blogRepo.GetBlogByIdAsync(id);

            if (blog == null || doctorDto == null || disease == null)
            {
                return NotFound();
            }
            if (updatedBlog == null)
                return BadRequest(ModelState);
            if (id != updatedBlog.Id)
                return BadRequest();

			if (updatedBlog.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(updatedBlog.ImageFile, "images/blogs/");
				if (fileResult.Item1 == 1)
				{
					updatedBlog.Image = fileResult.Item2;
					await _fileService.DeleteImage(blog.Image);
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			await _blogRepo.UpdateBlogAsync(updatedBlog);

            return Ok("Update Successfully!");
        }

        [Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteBlog([FromQuery] int id)
        {
            var blog = await _blogRepo.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            await _blogRepo.DeleteBlogAsync(id);
			await _fileService.DeleteImage(blog.Image);

			return Ok("Delete Successfully!");
        }
    }
}
