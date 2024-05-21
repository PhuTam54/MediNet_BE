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

namespace MediNet_BE.Controllers.Employees.Blogs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepo _blogRepo;
        private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;
        private readonly IDiseaseRepo _diseaseRepo;
        private readonly IMapper _mapper;

        public BlogsController(IBlogRepo blogRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo, IDiseaseRepo diseaseRepo, IMapper mapper)
        {
            _blogRepo = blogRepo;
            _employeeRepo = employeeRepo;
            _diseaseRepo = diseaseRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetBlogs()
        {
            return Ok(await _blogRepo.GetAllBlogAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<BlogDto>> GetBlogById([FromQuery] int id)
        {
            var blog = await _blogRepo.GetBlogByIdAsync(id);
            return blog == null ? NotFound() : Ok(blog);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Doctor")]
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] BlogCreate blogCreate)
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

            var newBlog = await _blogRepo.AddBlogAsync(blogCreate);
            return newBlog == null ? NotFound() : Ok(newBlog);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Doctor")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateBlog([FromQuery] int id, [FromBody] BlogCreate updatedBlog)
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

            await _blogRepo.UpdateBlogAsync(updatedBlog);

            return Ok("Update Successfully!");
        }

        [Authorize]
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
            return Ok("Delete Successfully!");
        }
    }
}
