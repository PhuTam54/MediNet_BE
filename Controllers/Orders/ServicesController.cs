using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.DtoCreate.Orders.OrderServices;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces;
using MediNet_BE.Dto.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Models.Employees;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepo _serviceRepo;
        private readonly IUserRepo<Employee, EmployeeDto, EmployeeCreate> _employeeRepo;

        public ServicesController(IServiceRepo serviceRepo, IUserRepo<Employee, EmployeeDto, EmployeeCreate> employeeRepo)
        {
            _serviceRepo = serviceRepo;
            _employeeRepo = employeeRepo;
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
            var service = await _serviceRepo.GetServiceByIdAsync(id);
            return service == null ? NotFound() : Ok(service);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Service>> CreateService([FromBody] ServiceCreate serviceCreate)
        {
            var employeeDoctor = await _employeeRepo.GetUserByIdAsync(serviceCreate.EmployeeId);
            if (employeeDoctor == null)
                return NotFound("Doctor Not Found!");
            if (serviceCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newService = await _serviceRepo.AddServiceAsync(serviceCreate);
            return newService == null ? NotFound() : Ok(newService);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateService([FromQuery] int id, [FromBody] ServiceCreate updatedService)
        {
            var employeeDoctor = await _employeeRepo.GetUserByIdAsync(updatedService.EmployeeId);
            if (employeeDoctor == null)
                return NotFound("Doctor Not Found!");
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
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
