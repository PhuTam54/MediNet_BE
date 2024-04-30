using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Users;
using MediNet_BE.Identity;
using MediNet_BE.Models.Users;
using MediNet_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediNet_BE.Controllers.Users
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class LoginRegisterController : ControllerBase
	{
		private readonly MediNetContext _context;

		private readonly IConfiguration _config;
		private readonly IMapper _mapper;
		private readonly IMailService _mailService;
		const int CUSTOMER = 1;
		const int ADMIN = 2;
		const int DOCTOR = 3;

        public LoginRegisterController(MediNetContext context, IConfiguration config, IMapper mapper, IMailService mailService)
		{
			_context = context;
			_config = config;
			_mapper = mapper;
			_mailService = mailService;
		}

		/// <summary>
		/// Login As a Admin
		/// </summary>
		/// <param name="loginModel"></param>
		/// <remarks>
		/// "email": "admin@gmail.com",
		/// "password": "123456"
		/// </remarks>
		/// <returns></returns>
		// JWT Authentication
		[AllowAnonymous]
		[HttpPost]
		[Route("Login")]
		public IActionResult Login([FromBody] AuthenticateRequest loginModel)
		{
			IActionResult response = Unauthorized();
			{
				var email = loginModel.Email;

				var userType = GetUserTypeByEmail(email);

				if (userType == UserType.Unknown)
				{
					return BadRequest("Invalid email or password.");
				}

				var account = FindUserByEmailAndType(email, userType);

				if (account != null && VerifyPassword(loginModel.Password, account.Password))
				{
					var userRole = "Guest";
					if (account.Role == CUSTOMER)
					{
						userRole = "Customer";
					}
					else if (account.Role == ADMIN)
					{
						userRole = "Admin";
					}

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Email, account.Email),
						new Claim(IdentityData.UserIdClaimName, account.Id.ToString()),
						new Claim(IdentityData.RoleClaimName, userRole)
                    };
					var token = GenerateJwtToken(_config["JwtSettings:SecretKey"],
						_config["JwtSettings:Issuer"], _config["JwtSettings:Audience"],
						int.Parse(_config["JwtSettings:ExpirationMinutes"]), claims);

					Response.Cookies.Append("jwt", token, new CookieOptions
					{
						HttpOnly = true,
						SameSite = SameSiteMode.Strict,
						Secure = true
					});

					return Ok(new { Token = token });
				}
			}

			return response;
		}

		private string GenerateJwtToken(string secretKey, string issuer, string audience, int expirationMinutes, IEnumerable<Claim> claims)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		/// <summary>
		/// Register information
		/// </summary>
		/// <param name="registerModel"></param>
		/// <remarks>
		/// {
		/// "email": "newuser@gmail.com (muốn test send email thì dùng email thật của mình)",
		/// "username": "newuser",
		/// "password": "123456",
		/// "confirmpassword": "123456"
		/// }
		/// </remarks>
		/// <returns></returns>
		// Resgiter
		[AllowAnonymous]
		[HttpPost]
		//[ValidateAntiForgeryToken]
		[Route("Register")]
		public IActionResult Register([FromBody] RegisterRequest registerModel)
		{
			if (_context.Customers.Any(x => x.Email == registerModel.Email))
				throw new ApplicationException("Email '" + registerModel.Email + "' is already taken");

			var user = _mapper.Map<Customer>(registerModel);

			user.Password = HashPassword(registerModel.Password);
			user.Slug = registerModel.UserName;
			user.Role = 1;
			user.Status = 0;
			user.Address = "";
			user.Gender = 0;
            user.Date_Of_Birth = DateTime.UtcNow;
			user.PhoneNumber = "";

			_context.Customers.Add(user);
			_context.SaveChanges();

			return Ok("Registration successful");
		}

		private UserType GetUserTypeByEmail(string email)
		{
			if (_context.Customers.Any(u => u.Email == email))
			{
				return UserType.Customer;
			}
			else if (_context.Admins.Any(u => u.Email == email))
			{
				return UserType.Admin;
			}
			else
			{
				return UserType.Unknown;
			}
		}
		private User FindUserByEmailAndType(string email, UserType userType)
		{
			switch (userType)
			{
				case UserType.Customer:
					return _context.Customers.FirstOrDefault(u => u.Email == email);
				case UserType.Admin:
					return _context.Admins.FirstOrDefault(u => u.Email == email);
				default:
					return null;
			}
		}

		public static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		private bool VerifyPassword(string enteredPassword, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
		}

		public enum UserType
		{
			Unknown,
			Customer,
			Admin,
			Doctor
		}

		[HttpPost]
		[Route("ForgotPwd")]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if(email == null)
			{
				return NotFound("Email is required!");
			}
			var account = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

			if(account != null)
			{
				var data = new SendMailRequest
				{
					ToEmail = account.Email,
					UserName = account.Username,
					Url = "forgotpwd",
					Subject = "Forgot Password"
				};
				await _mailService.SendEmailAsync(data);
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		[Route("ResetPwd")]
		public async Task<IActionResult> ResetPassword(int? userId, string pwd, string confirmpwd)
		{
			if (userId == null)
			{
				return NotFound();
			}
			var account = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId);

			if (account != null)
			{
				if (pwd != confirmpwd)
				{
					return BadRequest("Confirm Password and Password are not the same.");
				}
				else
				{
					account.Password = HashPassword(pwd);
					_context.Customers.Update(account);
					await _context.SaveChangesAsync();
					return Ok("Resetpwd success!");
				}
			}
			else
			{
				return NotFound();
			}


		}


	}
}
