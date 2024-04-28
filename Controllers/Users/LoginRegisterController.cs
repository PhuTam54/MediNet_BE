using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Users;
using MediNet_BE.Identity;
using MediNet_BE.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

		const int CUSTOMER = 1;
		const int ADMIN = 2;

		public LoginRegisterController(MediNetContext context, IConfiguration config, IMapper mapper)
		{
			_context = context;
			_config = config;
			_mapper = mapper;
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

				// Xác định loại người dùng từ email
				var userType = GetUserTypeByEmail(email);

				// Nếu không xác định được loại người dùng
				if (userType == UserType.Unknown)
				{
					return BadRequest("Invalid email or password.");
				}

				// Thực hiện truy vấn đăng nhập tương ứng
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
					// Tạo danh sách claim

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Email, account.Email),
						new Claim(IdentityData.UserIdClaimName, account.Id.ToString()),
						new Claim(IdentityData.RoleClaimName, userRole)
                        // 1.Customer / 2.Admin 
                    };
					// Tạo JWT token
					var token = GenerateJwtToken(_config["JwtSettings:SecretKey"],
						_config["JwtSettings:Issuer"], _config["JwtSettings:Audience"],
						int.Parse(_config["JwtSettings:ExpirationMinutes"]), claims);

					// Lưu JWT token vào cookie
					Response.Cookies.Append("jwt", token, new CookieOptions
					{
						HttpOnly = true,
						SameSite = SameSiteMode.Strict,
						Secure = true // Đặt true nếu bạn chỉ muốn gửi cookie qua kết nối HTTPS
					});

					// Trả về JWT token cho người dùng
					return Ok(new { Token = token });
				}
			}

			return response; // Trả về Unauthorized nếu xác thực không thành công
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
			// validate
			if (_context.Customers.Any(x => x.Email == registerModel.Email))
				throw new ApplicationException("Email '" + registerModel.Email + "' is already taken");

			// map model to new user object
			var user = _mapper.Map<Customer>(registerModel);

			// Hash the password before saving it
			user.Password = HashPassword(registerModel.Password);
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
			// Kiểm tra email thuộc về loại người dùng nào
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
			// So sánh mật khẩu đã hash trong cơ sở dữ liệu với mật khẩu người dùng nhập vào
			return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
		}

		public enum UserType
		{
			Unknown,
			Customer,
			Admin,
			Tenant
		}
	}
}
