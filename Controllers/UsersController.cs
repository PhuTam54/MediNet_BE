using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models;
using AutoMapper;
using MediNet_BE.Repositories;
using MediNet_BE.Interfaces;
using MediNet_BE.Dto;
using MediNet_BE.Services.Image;

namespace MediNet_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		//private readonly IUserRepo _userRepo;
		//private readonly IFileService _fileService;

		//public UsersController(IUserRepo userRepo,IFileService fileService)
		//{
		//	_userRepo = userRepo;
		//	_fileService = fileService;
		//}

		//[HttpGet]
		//public async Task<IActionResult> GetUsers()
		//{
		//	return Ok(await _userRepo.getAllUserAsync());
		//}

		//[HttpGet]
		//[Route("id")]
		//public async Task<IActionResult> GetUserById(int id)
		//{
		//	var user = await _userRepo.getUserByIdAsync(id);
		//	return user == null ? NotFound() : Ok(user);
		//}

		//[HttpPost]
		//public async Task<ActionResult> CreateUser([FromForm] UserDto userCreate)
		//{
		//	if (userCreate == null)
		//		return BadRequest(ModelState);

		//	if (!ModelState.IsValid)
		//		return BadRequest(ModelState);

		//	if (userCreate.ImageFile != null)
		//	{
		//		var fileResult = _fileService.SaveImage(userCreate.ImageFile);
		//		if (fileResult.Item1 == 1)
		//		{
		//			userCreate.Image = fileResult.Item2;
		//		}
		//		else
		//		{
		//			return NotFound("An error occurred while saving the image!");
		//		}
		//	}
		//	var newUser = await _userRepo.AddUserAsync(userCreate);
		//	return newUser == null ? NotFound() : Ok(newUser);
		//}

		//[HttpPut]
		//[Route("id")]
		//public async Task<IActionResult> UpdateUser([FromQuery]int id, [FromForm] UserDto updatedUser)
		//{
		//	var user = await _userRepo.getUserByIdAsync(id);
		//	if (user == null)
		//		return NotFound();
		//	if (updatedUser == null)
		//		return BadRequest(ModelState);
		//	if (id != updatedUser.Id)
		//		return BadRequest();

		//	if (updatedUser.ImageFile != null)
		//	{
		//		var fileResult = _fileService.SaveImage(updatedUser.ImageFile);
		//		if (fileResult.Item1 == 1)
		//		{
		//			updatedUser.Image = fileResult.Item2;
		//			await _fileService.DeleteImage(user.Image);
		//		}
		//		else
		//		{
		//			return NotFound("An error occurred while saving the image!");
		//		}
		//	}
		//	await _userRepo.UpdateUserAsync(updatedUser);

		//	return Ok("Update Successfully!");
		//}

		//[HttpDelete]
		//[Route("id")]
		//public async Task<IActionResult> DeleteUser([FromQuery] int id)
		//{
		//	var user = await _userRepo.getUserByIdAsync(id);
		//	if (user == null)
		//	{
		//		return NotFound();
		//	}
		//	await _fileService.DeleteImage(user.Image);
		//	await _userRepo.DeleteUserAsync(user);
		//	return Ok("Delete Successfully!");
		//}

	}
}
