using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces;
using MediNet_BE.Services.Image;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Dto.Employees;

namespace MediNet_BE.Controllers.Employees
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _userRepo;
        private readonly IFileService _fileService;
        private readonly ISpecialistRepo _specialistRepo;
        private readonly IClinicRepo _clinicRepo;

        public EmployeesController(IUserRepo<Employee, EmployeeDto, EmployeeCreate> userRepo, IFileService fileService, ISpecialistRepo specialistRepo, IClinicRepo clinicRepo)
        {
            _userRepo = userRepo;
            _fileService = fileService;
            _specialistRepo = specialistRepo;
            _clinicRepo = clinicRepo;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _userRepo.GetAllUserAsync();
            foreach (var employee in employees)
            {
                employee.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, employee.Image);
            }
            return Ok(employees);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _userRepo.GetUserByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, employee.Image);

            return Ok(employee);
        }

        [HttpGet]
        [Route("email")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByEmail(string email)
        {
            var employee = await _userRepo.GetUserByEmailAsync(email);
            if (employee == null)
            {
                return NotFound();
            }
            employee.ImageSrc = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, employee.Image);

            return Ok(employee);
        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="userCreate"></param>
        /// <remarks>
        /// "email": "employee@gmail.com",
        /// "username": "employee",
        /// "password": "123456",
        /// "role": 3
        /// </remarks>
        /// <returns></returns>

        //[Authorize]
        //[RequiresClaim(IdentityData.RoleClaimName, "Employee")]
        // For the test -> When deploy delete the AllowAnonymous
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromForm] EmployeeCreate userCreate)
        {
            var specialist = await _specialistRepo.GetSpecialistByIdAsync(userCreate.SpecialistId);
            var clinic = await _clinicRepo.GetClinicByIdAsync(userCreate.ClinicId);
            if (specialist == null || clinic == null)
            {
                return NotFound();
            }

            if (userCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (userCreate.ImageFile != null)
            {
				var imgPath = "";
				if (userCreate.Role == 3)
					imgPath = "images/users/doctors/";
				if (userCreate.Role == 4)
					imgPath = "images/users/employees/";

				var fileResult = _fileService.SaveImage(userCreate.ImageFile, imgPath);

				if (fileResult.Item1 == 1)
                {
                    userCreate.Image = fileResult.Item2;
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }
            var newUser = await _userRepo.AddUserAsync(userCreate);
            return newUser == null ? NotFound() : Ok(newUser);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateEmployee([FromQuery] int id, [FromForm] EmployeeCreate updatedUser)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            var specialist = await _specialistRepo.GetSpecialistByIdAsync(updatedUser.SpecialistId);
            var clinic = await _clinicRepo.GetClinicByIdAsync(updatedUser.ClinicId);
            if (specialist == null || clinic == null)
            {
                return NotFound();
            }
            if (user == null)
                return NotFound();
            if (updatedUser == null)
                return BadRequest(ModelState);
            if (id != updatedUser.Id)
                return BadRequest();
            if (updatedUser.ImageFile != null)
            {
                var imgPath = "";
                if(updatedUser.Role == 3)
                    imgPath = "images/users/doctors/";
                if (updatedUser.Role == 4)
                    imgPath = "images/users/employees/";

				var fileResult = _fileService.SaveImage(updatedUser.ImageFile, imgPath);
                if (fileResult.Item1 == 1)
                {
                    updatedUser.Image = fileResult.Item2;
                    await _fileService.DeleteImage(user.Image);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }
            await _userRepo.UpdateUserAsync(updatedUser);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteEmployee([FromQuery] int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userRepo.DeleteUserAsync(id);
            await _fileService.DeleteImage(user.Image);
            return Ok("Delete Successfully!");
        }
    }
}
