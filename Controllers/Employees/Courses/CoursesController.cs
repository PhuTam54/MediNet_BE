using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Employees;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.Models.Employees;
using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Interfaces.Employees.Courses;

namespace MediNet_BE.Controllers.Employees.Courses
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ICourseRepo _courseRepo;
		private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;

		public CoursesController(ICourseRepo courseRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo)
		{
			_courseRepo = courseRepo;
			_employeeRepo = employeeRepo;
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
		[RequiresClaim(IdentityData.RoleClaimName, "Employee")]
		[HttpPost]
		public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseCreate courseCreate)
		{
			var employeeDoctor = await _employeeRepo.GetUserByIdAsync(courseCreate.EmployeeId);
			if (employeeDoctor == null || (employeeDoctor != null && employeeDoctor.RoleEmployee != 1))
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
		[RequiresClaim(IdentityData.RoleClaimName, "Employee")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCourse([FromQuery] int id, [FromBody] CourseCreate updatedCourse)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			var employeeDoctor = await _employeeRepo.GetUserByIdAsync(updatedCourse.EmployeeId);
			if (employeeDoctor == null || course == null || (employeeDoctor != null && employeeDoctor.RoleEmployee != 1))
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
