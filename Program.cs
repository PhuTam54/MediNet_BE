using MediNet_BE.Data;
using MediNet_BE.Dto.Employees;
using MediNet_BE.Dto.Mails;
using MediNet_BE.Dto.Payments.Momo;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Interfaces;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Interfaces.Employees;
using MediNet_BE.Interfaces.Employees.Blogs;
using MediNet_BE.Interfaces.Employees.Courses;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Interfaces.Products;
using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Users;
using MediNet_BE.Repositories.Categories;
using MediNet_BE.Repositories.Clinics;
using MediNet_BE.Repositories.Employees;
using MediNet_BE.Repositories.Employees.Blogs;
using MediNet_BE.Repositories.Employees.Courses;
using MediNet_BE.Repositories.Orders;
using MediNet_BE.Repositories.Products;
using MediNet_BE.Repositories.Users;
using MediNet_BE.Services;
using MediNet_BE.Services.Image;
using MediNet_BE.Services.Momo;
using MediNet_BE.Services.PayPal;
using MediNet_BE.Services.VNPay;
using MediNet_BE.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
	{
		Version = "v1",
		Title = "MediNet API",
		Description = "MediNet API",
	});
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//add repository
builder.Services.AddScoped<ICategoryParentRepo, CategoryParentRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryChildRepo, CategoryChildRepo>();
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<IInStockRepo, InStockRepo>();
builder.Services.AddScoped<IStockInRepo, StockInRepo>();
builder.Services.AddScoped<IStockOutRepo, StockOutRepo>();
builder.Services.AddScoped<ICourseRepo, CourseRepo>();
builder.Services.AddScoped<IEnrollmentRepo, EnrollmentRepo>();
builder.Services.AddScoped<IBlogRepo, BlogRepo>();
builder.Services.AddScoped<IBlogCommentRepo, BlogCommentRepo>();
builder.Services.AddScoped<IDiseaseRepo, DiseaseRepo>();
builder.Services.AddScoped<ISpecialistRepo, SpecialistRepo>();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductDetailRepo, ProductDetailRepo>();
builder.Services.AddScoped<IFavoriteProductRepo, FavoriteProductRepo>();

builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IFeedbackRepo, FeedbackRepo>();

builder.Services.AddScoped<IUserRepo<Customer, CustomerDto, CustomerCreate>, CustomerRepo>();
builder.Services.AddScoped<IUserRepo<Admin, AdminDto, AdminCreate>, AdminRepo>();
builder.Services.AddScoped<IUserRepo<Employee, EmployeeDto, EmployeeCreate>, EmployeeRepo>();


builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var configuration = builder.Configuration;
builder.Services.AddDbContext<MediNetContext>(opt =>
	opt.UseSqlServer(configuration.GetConnectionString("MediNetContext") ??
	throw new InvalidOperationException("Connection string 'MediNetContext' not found."))
);

//add service
builder.Services.Configure<MomoOptionModel>(configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<IPayPalService, PayPalService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();


//authenciation
var jwtSettings = configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
	};
});


var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();

// Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
