using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.Interfaces.Employees.Blogs;

namespace MediNet_BE.Controllers.Employees.Blogs
{
    [Route("api/v1/[controller]")]
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
        public async Task<ActionResult<DiseaseDto>> GetDiseaseById([FromQuery] int id)
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
