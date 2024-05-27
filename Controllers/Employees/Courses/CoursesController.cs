using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.Models.Employees;
using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Services.Image;
using MediNet_BE.Interfaces.Employees.Courses;
using AutoMapper;

namespace MediNet_BE.Controllers.Employees.Courses
{
    [Route("api/v1/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ICourseRepo _courseRepo;
		private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;

		public CoursesController(ICourseRepo courseRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo, IFileService fileService, IMapper mapper)
		{
			_courseRepo = courseRepo;
			_employeeRepo = employeeRepo;
			_fileService = fileService;
			_mapper = mapper;
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
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
		{
			var courses = await _courseRepo.GetAllCourseAsync();
			foreach (var course in courses)
			{
				course.ImagesSrc.AddRange(GetImagesPath(course.ImagesCourse));
			}
			return Ok(courses);
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<CourseDto>> GetCourseById(int id)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			if (course == null)
			{
				return NotFound();
			}
			course.ImagesSrc.AddRange(GetImagesPath(course.ImagesCourse));
			return Ok(course);
		}

		[HttpGet]
		[Route("employeeId")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByEmployeeId(int employeeId)
		{
			var employeeDto = await _employeeRepo.GetUserByIdAsync(employeeId);
			if (employeeDto == null)
			{
				return NotFound();
			}
			var courses = await _courseRepo.GetCoursesByEmployeeIdAsync(employeeId);
			foreach (var course in courses)
			{
				course.ImagesSrc.AddRange(GetImagesPath(course.ImagesCourse));
			}
			return Ok(courses);
		}

		[HttpGet]
		[Route("doctorId")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByDoctorId(int doctorId)
		{
			var employeeDto = await _employeeRepo.GetUserByIdAsync(doctorId);
			if (employeeDto == null || (employeeDto != null && employeeDto.Role != 3))
			{
				return NotFound();
			}
			var courses = await _courseRepo.GetCoursesByDoctorIdAsync(doctorId);
			foreach (var course in courses)
			{
				course.ImagesSrc.AddRange(GetImagesPath(course.ImagesCourse));
			}
			return Ok(courses);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPost]
		public async Task<ActionResult<Course>> CreateCourse([FromForm] CourseCreate courseCreate)
		{
			var doctorDto = await _employeeRepo.GetUserByIdAsync(courseCreate.EmployeeId);
			if (doctorDto == null)
			{
				return NotFound();
			}
			if (courseCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (courseCreate.ImagesCourseFile != null)
			{
				var fileResult = _fileService.SaveImages(courseCreate.ImagesCourseFile, "images/courses/");
				if (fileResult.Item1 == 1)
				{
					courseCreate.ImagesCourse = fileResult.Item2;
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

			var newCourse = await _courseRepo.AddCourseAsync(courseCreate);
			return newCourse == null ? NotFound() : Ok(newCourse);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateCourse([FromQuery] int id, [FromForm] CourseCreate updatedCourse)
		{
			var course = await _courseRepo.GetCourseByIdAsync(id);
			var doctorDto = await _employeeRepo.GetUserByIdAsync(updatedCourse.EmployeeId);
			if (doctorDto == null || course == null)
			{
				return NotFound();
			}

			if (updatedCourse == null)
				return BadRequest(ModelState);
			if (id != updatedCourse.Id)
				return BadRequest();

			if (updatedCourse.ImagesCourseFile != null)
			{
				var fileResult = _fileService.SaveImages(updatedCourse.ImagesCourseFile, "images/courses/");
				if (fileResult.Item1 == 1)
				{
					updatedCourse.ImagesCourse = fileResult.Item2;
					await _fileService.DeleteImages(course.ImagesCourse);
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}

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
			await _fileService.DeleteImages(course.ImagesCourse);

			return Ok("Delete Successfully!");
		}
	}
}
