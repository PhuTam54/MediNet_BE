using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models.Clinics;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Enrollments;
using MediNet_BE.Interfaces.Courses;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Interfaces;
using MediNet_BE.Models.Users;
using MediNet_BE.Repositories.Courses;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.Models.Employees;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.Dto.Employees;

namespace MediNet_BE.Controllers.Employees.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentRepo _enrollmentRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;

        public EnrollmentsController(IEnrollmentRepo enrollmentRepo, ICourseRepo courseRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _courseRepo = courseRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetCategories()
        {
            return Ok(await _enrollmentRepo.GetAllEnrollmentAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<EnrollmentDto>> GetEnrollmentById(int id)
        {
            var enrollment = await _enrollmentRepo.GetEnrollmentByIdAsync(id);
            return enrollment == null ? NotFound() : Ok(enrollment);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Enrollment>> CreateEnrollment([FromBody] EnrollmentCreate enrollmentCreate)
        {
            var course = await _courseRepo.GetCourseByIdAsync(enrollmentCreate.CourseId);
            var employee = await _employeeRepo.GetUserByIdAsync(enrollmentCreate.EmployeeId);
            if (course == null || employee == null)
            {
                return NotFound();
            }
            if (enrollmentCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEnrollment = await _enrollmentRepo.AddEnrollmentAsync(enrollmentCreate);
            return newEnrollment == null ? NotFound() : Ok(newEnrollment);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateEnrollment([FromQuery] int id, [FromBody] EnrollmentCreate updatedEnrollment)
        {
            var enrollment = await _enrollmentRepo.GetEnrollmentByIdAsync(id);
            var course = await _courseRepo.GetCourseByIdAsync(updatedEnrollment.CourseId);
            var employee = await _employeeRepo.GetUserByIdAsync(updatedEnrollment.EmployeeId);
            if (course == null || employee == null)
            {
                return NotFound();
            }
            if (enrollment == null)
                return NotFound();
            if (updatedEnrollment == null)
                return BadRequest(ModelState);
            if (id != updatedEnrollment.Id)
                return BadRequest();

            await _enrollmentRepo.UpdateEnrollmentAsync(updatedEnrollment);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteEnrollment([FromQuery] int id)
        {
            var enrollment = await _enrollmentRepo.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            await _enrollmentRepo.DeleteEnrollmentAsync(id);
            return Ok("Delete Successfully!");
        }
    }
}
