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
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Repositories.Clinics;

namespace MediNet_BE.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialistsController : ControllerBase
    {
        private readonly ISpecialistRepo _specialistRepo;

        public SpecialistsController(ISpecialistRepo specialistRepo)
        {
            _specialistRepo = specialistRepo;

		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialistDto>>> GetSpecialists()
        {
            return Ok(await _specialistRepo.GetAllSpecialistAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<SpecialistDto>> GetSpecialist([FromQuery] int id)
        {
            var specialist = await _specialistRepo.GetSpecialistByIdAsync(id);
            return specialist == null ? NotFound() : Ok(specialist);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Specialist>> CreateSpecialist([FromBody] SpecialistCreate specialistCreate)
        {
            if (specialistCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newSpecialist = await _specialistRepo.AddSpecialistAsync(specialistCreate);
            return newSpecialist == null ? NotFound() : Ok(newSpecialist);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateSpecialist([FromQuery] int id, [FromBody] SpecialistCreate updatedSpecialist)
        {
            var specialist = await _specialistRepo.GetSpecialistByIdAsync(id);
            if (specialist == null)
                return NotFound();
            if (updatedSpecialist == null)
                return BadRequest(ModelState);
            if (id != updatedSpecialist.Id)
                return BadRequest();

            await _specialistRepo.UpdateSpecialistAsync(updatedSpecialist);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteSpecialist([FromQuery] int id)
        {
            var specialist = await _specialistRepo.GetSpecialistByIdAsync(id);
            if (specialist == null)
            {
                return NotFound();
            }
            await _specialistRepo.DeleteSpecialistAsync(id);
            return Ok("Delete Successfully!");
        }
    }
}
