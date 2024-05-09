using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Repositories.Doctors;
using MediNet_BE.Interfaces;
using MediNet_BE.Services.Image;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Dto.Doctors;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Repositories.Clinics;
using MediNet_BE.DtoCreate.Users;

namespace MediNet_BE.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IUserRepo<Doctor, DoctorDto, DoctorCreate> _doctorRepo;
		private readonly IFileService _fileService;
		private readonly ISpecialistRepo _specialistRepo;
		private readonly IClinicRepo _clinicRepo;

		public DoctorsController(IUserRepo<Doctor, DoctorDto, DoctorCreate> doctorRepo, IFileService fileService, ISpecialistRepo specialistRepo, IClinicRepo clinicRepo)
		{
			_doctorRepo = doctorRepo;
			_fileService = fileService;
			_specialistRepo = specialistRepo;
			_clinicRepo = clinicRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
		{
			var doctors = await _doctorRepo.GetAllUserAsync();
			foreach (var doctor in doctors)
			{
				doctor.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, doctor.Image);
			}
			return Ok(doctors);
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<DoctorDto>> GetDoctorById(int id)
		{
			var doctor = await _doctorRepo.GetUserByIdAsync(id);
			if(doctor == null)
			{
				return NotFound();
			}
			doctor.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, doctor.Image);

			return Ok(doctor);
		}

		[HttpGet]
		[Route("email")]
		public async Task<ActionResult<DoctorDto>> GetDoctorByEmail(string email)
		{
			var doctor = await _doctorRepo.GetUserByEmailAsync(email);
			if (doctor == null)
			{
				return NotFound();
			}
			doctor.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, doctor.Image);

			return Ok(doctor);
		}

        /// <summary>
        /// Create Doctor
        /// </summary>
        /// <param name="userCreate"></param>
        /// <remarks>
        /// "email": "doctor@gmail.com",
        /// "username": "doctor",
        /// "password": "123456",
        /// "role": 3
        /// </remarks>
        /// <returns></returns>

        //[Authorize]
        //[RequiresClaim(IdentityData.RoleClaimName, "Doctor")]
		// For the test -> When deploy delete the AllowAnonymous
        [AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<Doctor>> CreateDoctor([FromForm] DoctorCreate userCreate)
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
				var fileResult = _fileService.SaveImage(userCreate.ImageFile, "images/users/doctors/");
				if (fileResult.Item1 == 1)
				{
					userCreate.Image = fileResult.Item2;
				}
				else
				{
					return NotFound("An error occurred while saving the image!");
				}
			}
			var newUser = await _doctorRepo.AddUserAsync(userCreate);
			return newUser == null ? NotFound() : Ok(newUser);
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateDoctor([FromQuery] int id, [FromForm] DoctorCreate updatedUser)
		{
			var user = await _doctorRepo.GetUserByIdAsync(id);
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
				var fileResult = _fileService.SaveImage(updatedUser.ImageFile, "images/users/doctors/");
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
			await _doctorRepo.UpdateUserAsync(updatedUser);

			return Ok("Update Successfully!");
		}

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteDoctor([FromQuery] int id)
		{
			var user = await _doctorRepo.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			await _doctorRepo.DeleteUserAsync(id);
			await _fileService.DeleteImage(user.Image);
			return Ok("Delete Successfully!");
		}
    }
}
