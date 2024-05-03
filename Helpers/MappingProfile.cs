using AutoMapper;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Helpers
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
			CreateMap<Customer, CustomerDto>();
			CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, RegisterRequest>();
            CreateMap<RegisterRequest, Customer>();
            CreateMap<Customer, AuthenticateRequest>();
            CreateMap<AuthenticateRequest, Customer>();
            CreateMap<Customer, CustomerReturnDto>();
            CreateMap<CustomerReturnDto, Customer>();

            CreateMap<Admin, AdminDto>();
			CreateMap<AdminDto, Admin>();

			CreateMap<Category, CategoryDto>();
			CreateMap<CategoryDto, Category>();

			CreateMap<CategoryChild, CategoryChildDto>();
			CreateMap<CategoryChildDto, CategoryChild>();

			CreateMap<Clinic, ClinicDto>();
			CreateMap<ClinicDto, Clinic>();
			CreateMap<Supply, SupplyDto>();
			CreateMap<SupplyDto, Supply>();

			CreateMap<Cart, CartDto>();
			CreateMap<CartDto, Cart>();

			CreateMap<Order, OrderDto>();
			CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderReturnDto>();
            CreateMap<OrderReturnDto, Order>();

            CreateMap<OrderProduct, OrderProductDto>();
			CreateMap<OrderProductDto, OrderProduct>();
			CreateMap<OrderProduct, OrderProductReturnDto>();
            CreateMap<OrderProductReturnDto, OrderProduct>();

			CreateMap<OrderService, OrderServiceDto>();
			CreateMap<OrderServiceDto, OrderService>();
			CreateMap<OrderService, OrderServiceReturnDto>();
            CreateMap<OrderServiceReturnDto, OrderService>();

			CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();

            CreateMap<Service, ServiceDto>();
			CreateMap<ServiceDto, Service>();
            CreateMap<Service, ServiceCreateDto>();
            CreateMap<ServiceCreateDto, Service>();

            CreateMap<Feedback, FeedbackDto>();
			CreateMap<FeedbackDto, Feedback>();
		}
	}
}
