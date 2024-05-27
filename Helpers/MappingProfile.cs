using AutoMapper;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Employees;
using MediNet_BE.Dto.Employees.Blogs;
using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Dto.Products;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.DtoCreate.Employees;
using MediNet_BE.DtoCreate.Employees.Blogs;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.DtoCreate.Orders;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderServices;
using MediNet_BE.DtoCreate.Products;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Employees.Blogs;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Products;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Helpers
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
			#region Users

			#region UserCreate
			CreateMap<AdminCreate, Admin>();
			CreateMap<CustomerCreate, Customer>();

			CreateMap<FeedbackCreate, Feedback>();

			CreateMap<Customer, RegisterRequest>();
			CreateMap<RegisterRequest, Customer>();
			CreateMap<Customer, AuthenticateRequest>();
			CreateMap<AuthenticateRequest, Customer>();
			#endregion

			CreateMap<Customer, CustomerDto>();
			CreateMap<Admin, AdminDto>();
			CreateMap<Feedback, FeedbackDto>();
			#endregion

			#region Categories
			#region CategoriesCreate
			CreateMap<CategoryParentCreate, CategoryParent>();
			CreateMap<CategoryCreate, Category>();
			CreateMap<CategoryChildCreate, CategoryChild>();
			#endregion

			CreateMap<CategoryParent, CategoryParentDto>();
			CreateMap<Category, CategoryDto>();
			CreateMap<CategoryChild, CategoryChildDto>();
			#endregion


			#region Orders
			#region OrdersCreate
			CreateMap<CartCreate, Cart>();
			CreateMap<Cart, CartCreate>();
			CreateMap<OrderCreate, Order>();
			CreateMap<ServiceCreate, Service>();


			#endregion
			CreateMap<Cart, CartDto>();
			CreateMap<Order, OrderDto>();
			CreateMap<OrderDto, Order>();
			CreateMap<OrderProduct, OrderProductDto>();
			CreateMap<OrderService, OrderServiceDto>();
			CreateMap<Service, ServiceDto>();
			#endregion

			#region Products
			#region ProductsCreate
			CreateMap<ProductCreate, Product>();
			CreateMap<ProductDetailCreate, ProductDetail>();
			CreateMap<FavoriteProductCreate, FavoriteProduct>();
			#endregion

			CreateMap<Product, ProductDto>();
			CreateMap<ProductDetail, ProductDetailDto>();
			CreateMap<FavoriteProduct, FavoriteProductDto>();
			#endregion

			#region Clinics
			#region ClinicsCreate
			CreateMap<ClinicCreate, Clinic>();
			CreateMap<InStockCreate, InStock>();
			CreateMap<StockInCreate, StockIn>();
			CreateMap<StockOutCreate, StockOut>();

			#endregion
			CreateMap<Clinic, ClinicDto>();
			CreateMap<InStock, InStockDto>();
			CreateMap<StockIn, StockInDto>();
			CreateMap<StockOut, StockOutDto>();

			#endregion

			#region Doctors
			#region DoctorsCreate
			CreateMap<BlogCreate, Blog> ();
			CreateMap<DiseaseCreate, Disease>();
			CreateMap<SpecialistCreate, Specialist>();
			CreateMap<BlogCommentCreate, BlogComment>();

			#endregion
			CreateMap<Blog, BlogDto>();
			CreateMap<Disease, DiseaseDto>();
			CreateMap<Specialist, SpecialistDto>();
			CreateMap<BlogComment, BlogCommentDto>();

			#endregion

			#region Couses
			CreateMap<EmployeeCreate, Employee>();
			CreateMap<Employee, EmployeeDto>();
			CreateMap<EmployeeDto, Employee>();

			CreateMap<CourseCreate, Course>();
			CreateMap<Course, CourseDto>();

			CreateMap<EnrollmentCreate, Enrollment>();
			CreateMap<Enrollment, EnrollmentDto>();
			#endregion

		}
	}
}
