using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models;
using MediNet_BE.Interfaces;
using MediNet_BE.Repositories;
using MediNet_BE.Dto;

namespace MediNet_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
		private readonly IClinicRepo _clinicRepo;

		public ClinicsController(IClinicRepo clinicRepo)
        {
			_clinicRepo = clinicRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetClinics()
        {
			return Ok(await _clinicRepo.GetAllClinicAsync());
		}

        [HttpGet]
		[Route("id")]
		public async Task<ActionResult<Clinic>> GetClinic(int id)
        {
			var clinic = await _clinicRepo.GetClinicByIdAsync(id);
			return clinic == null ? NotFound() : Ok(clinic);
		}

		/// <summary>
		/// Create Clinic
		/// </summary>
		/// <param name="clinicCreate"></param>
		/// <remarks>
		///  "name": "Abc",
		///  "address": "Abc-123",
		///  "phone": "0987654321",
		///  "email": "abc@gmail.com"
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
        public async Task<ActionResult<Clinic>> CreateClinic(ClinicDto clinicCreate)
        {
			if (clinicCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newClinic = await _clinicRepo.AddClinicAsync(clinicCreate);
			return newClinic == null ? NotFound() : Ok(newClinic);
		}

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateClinic(int id, ClinicDto updatedClinic)
		{
			var clinic = await _clinicRepo.GetClinicByIdAsync(id);
			if (clinic == null)
				return NotFound();
			if (updatedClinic == null)
				return BadRequest(ModelState);
			if (id != updatedClinic.Id)
				return BadRequest();

			await _clinicRepo.UpdateClinicAsync(updatedClinic);

			return Ok("Update Successfully!");
		}

		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteClinic(int id)
        {
			var clinic = await _clinicRepo.GetClinicByIdAsync(id);
			if (clinic == null)
			{
				return NotFound();
			}
			await _clinicRepo.DeleteClinicAsync(clinic);
			return Ok("Delete Successfully!");
		}

    }
}
