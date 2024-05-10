using AutoMapper;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Courses;
using MediNet_BE.Dto.Doctors;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Categories;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.DtoCreate.Courses;
using MediNet_BE.DtoCreate.Doctors;
using MediNet_BE.DtoCreate.Orders;
using MediNet_BE.DtoCreate.Orders.OrderProducts;
using MediNet_BE.DtoCreate.Orders.OrderServices;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Courses;
using MediNet_BE.Models.Doctors;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
			CreateMap<ProductCreate, Product>();
			CreateMap<ProductDetailCreate, ProductDetail>();
			CreateMap<CartCreate, Cart>();
			CreateMap<OrderCreate, Order>();
			CreateMap<ServiceCreate, Service>();


			#endregion
			CreateMap<Cart, CartDto>();
			CreateMap<Order, OrderDto>();
			CreateMap<OrderProduct, OrderProductDto>();
			CreateMap<OrderService, OrderServiceDto>();
			CreateMap<Product, ProductDto>();
			CreateMap<ProductDetail, ProductDetailDto>();
			CreateMap<Service, ServiceDto>();
			#endregion

			#region Clinics
			#region ClinicsCreate
			CreateMap<ClinicCreate, Clinic>();
			CreateMap<SupplyCreate, Supply>();

			#endregion
			CreateMap<Clinic, ClinicDto>();
			CreateMap<Supply, SupplyDto>();
			#endregion

			#region Doctors
			#region DoctorsCreate
			CreateMap<DoctorCreate, Doctor>();
			CreateMap<BlogCreate, Blog> ();
			CreateMap<DiseaseCreate, Disease>();
			CreateMap<SpecialistCreate, Specialist>();

			#endregion
			CreateMap<Doctor, DoctorDto>();
			CreateMap<Blog, BlogDto>();
			CreateMap<Disease, DiseaseDto>();
			CreateMap<Specialist, SpecialistDto>();

			#endregion

			#region Couses
			CreateMap<EmployeeCreate, Employee>();
			CreateMap<Employee, EmployeeDto>();

			CreateMap<CourseCreate, Course>();
			CreateMap<Course, CourseDto>();

			CreateMap<EnrollmentCreate, Enrollment>();
			CreateMap<Enrollment, EnrollmentDto>();
			#endregion

		}
	}
}
