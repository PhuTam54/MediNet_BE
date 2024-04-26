using AutoMapper;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Helpers
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
			CreateMap<Customer, CustomerDto>();
			CreateMap<CustomerDto, Customer>();
			CreateMap<Admin, AdminDto>();
			CreateMap<AdminDto, Admin>();
			CreateMap<Category, CategoryDto>();
			CreateMap<CategoryDto, Category>();

			CreateMap<CategoryChild, CategoryChildDto>();
			CreateMap<CategoryChildDto, CategoryChild>();

			CreateMap<Clinic, ClinicDto>();
			CreateMap<ClinicDto, Clinic>();

			CreateMap<Cart, CartDto>();
			CreateMap<CartDto, Cart>();

			CreateMap<Order, OrderDto>();
			CreateMap<OrderDto, Order>();
			CreateMap<OrderProduct, OrderProductDto>();
			CreateMap<OrderProductDto, OrderProduct>();
			CreateMap<OrderService, OrderServiceDto>();
			CreateMap<OrderServiceDto, OrderService>();
			CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();
			CreateMap<Service, ServiceDto>();
			CreateMap<ServiceDto, Service>();
		}
	}
}
