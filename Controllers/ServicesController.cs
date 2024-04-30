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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
		private readonly IServiceRepo _serviceRepo;
		private readonly IClinicRepo _clinicRepo;

		public ServicesController(IServiceRepo serviceRepo, IClinicRepo clinicRepo)
        {
			_serviceRepo = serviceRepo;
			_clinicRepo = clinicRepo;

		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
        {
			return Ok(await _serviceRepo.GetAllServiceAsync());
		}

        [HttpGet]
		[Route("id")]
		public async Task<ActionResult<ServiceDto>> GetService([FromQuery] int id)
        {
			var serviceDto = await _serviceRepo.GetServiceByIdAsync(id);
			return serviceDto == null ? NotFound() : Ok(serviceDto);
		}

        [HttpPost]
        public async Task<ActionResult<Service>> CreateService([FromBody]ServiceDto serviceCreate)
        {
			var clinic = await _clinicRepo.GetClinicByIdAsync(serviceCreate.ClinicId);
			if (clinic == null)
				return NotFound("Clinic Not Found!");
			if (serviceCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newService = await _serviceRepo.AddServiceAsync(serviceCreate);
			return newService == null ? NotFound() : Ok(newService);
		}

		[HttpPut]
		[Route("id")]
		public async Task<IActionResult> UpdateService([FromQuery] int id, [FromBody] ServiceDto updatedService)
		{
			var clinic = await _clinicRepo.GetClinicByIdAsync(updatedService.ClinicId);
			if (clinic == null)
				return NotFound("Clinic Not Found!");
			var service = await _serviceRepo.GetServiceByIdAsync(id);
			if (service == null)
				return NotFound();
			if (updatedService == null)
				return BadRequest(ModelState);
			if (id != updatedService.Id)
				return BadRequest();

			await _serviceRepo.UpdateServiceAsync(updatedService);

			return Ok("Update Successfully!");
		}


		[HttpDelete]
		[Route("id")]
		public async Task<IActionResult> DeleteService([FromQuery] int id)
        {
			var service = await _serviceRepo.GetServiceByIdAsync(id);
			if (service == null)
			{
				return NotFound();
			}
			await _serviceRepo.DeleteServiceAsync(id);
			return Ok("Delete Successfully!");
		}

    }
}
