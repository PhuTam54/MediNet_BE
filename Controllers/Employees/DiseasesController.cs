using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Identity;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Models.Employees;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.Dto.Employees;

namespace MediNet_BE.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseasesController : ControllerBase
    {
        private readonly IDiseaseRepo _diseaseRepo;

        public DiseasesController(IDiseaseRepo diseaseRepo)
        {
            _diseaseRepo = diseaseRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseases()
        {
            return Ok(await _diseaseRepo.GetAllDiseaseAsync());
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<DiseaseDto>> GetDisease([FromQuery] int id)
        {
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(id);
            return disease == null ? NotFound() : Ok(disease);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Disease>> CreateDisease([FromBody] DiseaseCreate diseaseCreate)
        {
            if (diseaseCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newDisease = await _diseaseRepo.AddDiseaseAsync(diseaseCreate);
            return newDisease == null ? NotFound() : Ok(newDisease);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateDisease([FromQuery] int id, [FromBody] DiseaseCreate updatedDisease)
        {
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(id);
            if (disease == null)
                return NotFound();
            if (updatedDisease == null)
                return BadRequest(ModelState);
            if (id != updatedDisease.Id)
                return BadRequest();

            await _diseaseRepo.UpdateDiseaseAsync(updatedDisease);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteDisease([FromQuery] int id)
        {
            var disease = await _diseaseRepo.GetDiseaseByIdAsync(id);
            if (disease == null)
            {
                return NotFound();
            }
            await _diseaseRepo.DeleteDiseaseAsync(id);
            return Ok("Delete Successfully!");
        }
    }
}
