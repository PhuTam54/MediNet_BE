using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Users;
using MediNet_BE.Dto.Users;
using MediNet_BE.Interfaces;
using MediNet_BE.Services.Image;
using MediNet_BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using MediNet_BE.Identity;

namespace MediNet_BE.Controllers.Users
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
		private readonly IUserRepo<Admin, AdminDto> _userRepo;
		private readonly IFileService _fileService;

		public AdminsController(IUserRepo<Admin, AdminDto> userRepo, IFileService fileService)
		{
			_userRepo = userRepo;
			_fileService = fileService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Admin>>> GetUsers()
		{
			return Ok(await _userRepo.GetAllUserAsync());
		}

		[HttpGet]
		[Route("id")]
		public async Task<ActionResult<Admin>> GetUserById(int id)
		{
			var user = await _userRepo.GetUserByIdAsync(id);
			return user == null ? NotFound() : Ok(user);
		}

		/// <summary>
		/// Create Admin
		/// </summary>
		/// <param name="userCreate"></param>
		/// <remarks>
		/// "email": "admin@gmail.com",
		/// "username": "admin",
		/// "password": "123456",
		/// "role": 2
		/// </remarks>
		/// <returns></returns>
        [AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<Admin>> CreateUser([FromForm] AdminDto userCreate)
		{
			if (userCreate == null)
				return BadRequest(ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (userCreate.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(userCreate.ImageFile, "images/users/admins/");
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
		public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromForm] AdminDto updatedUser)
		{
			var user = await _userRepo.GetUserByIdAsync(id);
			if (user == null)
				return NotFound();
			if (updatedUser == null)
				return BadRequest(ModelState);
			if (id != updatedUser.Id)
				return BadRequest();
			if (updatedUser.ImageFile != null)
			{
				var fileResult = _fileService.SaveImage(updatedUser.ImageFile, "images/users/admins/");
				if (fileResult.Item1 == 1)
				{
					updatedUser.Image = fileResult.Item2;
					await _fileService.DeleteImage(user.Image, "images/users/admins/");
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
		public async Task<IActionResult> DeleteUser(int id)
		{
			var user = await _userRepo.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			await _userRepo.DeleteUserAsync(user);
			await _fileService.DeleteImage(user.Image, "images/users/admins/");
			return Ok("Delete Successfully!");
		}
	}
}
