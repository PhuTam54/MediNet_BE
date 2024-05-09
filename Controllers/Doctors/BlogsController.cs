using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Courses;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.Dto.Courses;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.Repositories.Doctors;

namespace MediNet_BE.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepo _blogRepo;
        private readonly IUserRepo<Doctor, DoctorDto, DoctorCreate> _doctorRepo;
        private readonly IDiseaseRepo _diseaseRepo;

        public BlogsController(IBlogRepo blogRepo, IUserRepo<Doctor, DoctorDto, DoctorCreate> doctorRepo, IDiseaseRepo diseaseRepo)
		{
            _blogRepo = blogRepo;
			_doctorRepo = doctorRepo;
            _diseaseRepo = diseaseRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetBlogs()
        {
            return Ok(await _blogRepo.GetAllBlogAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<BlogDto>> GetBlog([FromQuery] int id)
        {
            var blog = await _blogRepo.GetBlogByIdAsync(id);
            return blog == null ? NotFound() : Ok(blog);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] BlogCreate blogCreate)
        {
            var doctor = await _doctorRepo.GetUserByIdAsync(blogCreate.DoctorId);
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(blogCreate.DiseaseId);
            if (doctor == null || disease == null)
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
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateBlog([FromQuery] int id, [FromBody] BlogCreate updatedBlog)
        {
            var doctor = await _doctorRepo.GetUserByIdAsync(updatedBlog.DoctorId);
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(updatedBlog.DiseaseId);
            var blog = await _blogRepo.GetBlogByIdAsync(id);

            if (blog == null || doctor == null || disease == null)
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
            return Ok("Delete Successfully!");
        }
    }
}
