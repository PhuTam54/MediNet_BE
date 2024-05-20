using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediNet_BE.Services.Image;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.DtoCreate.Clinics;

namespace MediNet_BE.Controllers.Clinics
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicRepo _clinicRepo;
        private readonly IFileService _fileService;

        public ClinicsController(IClinicRepo clinicRepo, IFileService fileService)
        {
            _clinicRepo = clinicRepo;
            _fileService = fileService;

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
        public async Task<ActionResult<IEnumerable<ClinicDto>>> GetClinics()
        {
            var clinics = await _clinicRepo.GetAllClinicAsync();
            foreach (var clinic in clinics)
            {
				clinic.ImagesSrc.AddRange(GetImagesPath(clinic.ImagesClinic));
            }
            return Ok(clinics);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<ClinicDto>> GetClinic([FromQuery] int id)
        {
            var clinic = await _clinicRepo.GetClinicByIdAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
			clinic.ImagesSrc.AddRange(GetImagesPath(clinic.ImagesClinic));

            return Ok(clinic);
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

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Clinic>> CreateClinic([FromForm] ClinicCreate clinicCreate)
        {
            if (clinicCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (clinicCreate.ImagesClinicFile != null)
            {
                var fileResult = _fileService.SaveImages(clinicCreate.ImagesClinicFile, "images/clinics/");
                if (fileResult.Item1 == 1)
                {
                    clinicCreate.ImagesClinic = fileResult.Item2;
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            var newClinic = await _clinicRepo.AddClinicAsync(clinicCreate);
            return newClinic == null ? NotFound() : Ok(newClinic);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateClinic([FromQuery] int id, [FromForm] ClinicCreate updatedClinic)
        {
            var clinic = await _clinicRepo.GetClinicByIdAsync(id);
            if (clinic == null)
                return NotFound();
            if (updatedClinic == null)
                return BadRequest(ModelState);
            if (id != updatedClinic.Id)
                return BadRequest();

            if (updatedClinic.ImagesClinicFile != null)
            {
                var fileResult = _fileService.SaveImages(updatedClinic.ImagesClinicFile, "images/clinics/");
                if (fileResult.Item1 == 1)
                {
                    updatedClinic.ImagesClinic = fileResult.Item2;
                    await _fileService.DeleteImages(clinic.ImagesClinic);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            await _clinicRepo.UpdateClinicAsync(updatedClinic);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteClinic([FromQuery] int id)
        {
            var clinic = await _clinicRepo.GetClinicByIdAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
            await _clinicRepo.DeleteClinicAsync(id);
            await _fileService.DeleteImages(clinic.ImagesClinic);

            return Ok("Delete Successfully!");
        }

    }
}
