using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Courses;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Employees;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Courses;
using MediNet_BE.Dto.Courses;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Repositories.Doctors;

namespace MediNet_BE.Controllers.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
		private readonly ICourseRepo _courseRepo;
		private readonly IUserRepo<Doctor, DoctorDto, DoctorCreate> _doctorRepo;

		public CoursesController(ICourseRepo courseRepo, IUserRepo<Doctor, DoctorDto, DoctorCreate> doctorRepo)
		{
			_courseRepo = courseRepo;
			_doctorRepo = doctorRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
		{
			return Ok(await _courseRepo.GetAllCourseAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<CourseDto>> GetCourseById(int id)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			return course == null ? NotFound() : Ok(course);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseCreate courseCreate)
		{
			var doctor = await _doctorRepo.GetUserByIdAsync(courseCreate.DoctorId);
			if (doctor == null)
			{
				return NotFound();
			}
			if (courseCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newCourse = await _courseRepo.AddCourseAsync(courseCreate);
			return newCourse == null ? NotFound() : Ok(newCourse);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCourse([FromQuery] int id, [FromBody] CourseCreate updatedCourse)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			var doctor = await _doctorRepo.GetUserByIdAsync(updatedCourse.DoctorId);
			if (doctor == null || course == null)
			{
				return NotFound();
			}
			if (updatedCourse == null)
				return BadRequest(ModelState);
			if (id != updatedCourse.Id)
				return BadRequest();

			await _courseRepo.UpdateCourseAsync(updatedCourse);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteCourse([FromQuery] int id)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			if (course == null)
			{
				return NotFound();
			}
			await _courseRepo.DeleteCourseAsync(id);
			return Ok("Delete Successfully!");
		}
	}
}
