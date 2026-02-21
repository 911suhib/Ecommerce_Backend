using AutoMapper;
using EcommearceBackend.Business.src.Dtos.Category;
using EcommearceBackend.Business.src.Dtos.Order;
using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommerceBackend.Domain.Entities;

namespace EcommerceBackend.Framework.src
{
	public class AutoMapperProfile:Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, CreateUserDto>();
			CreateMap<CreateUserDto, User>();
			CreateMap<User, ReadUserDto>();
			CreateMap<ReadUserDto, User>();

			CreateMap<User, UpdateUserDto>();
			CreateMap<UpdateUserDto, User>();

			CreateMap<Product, CreateProductDto>();
			CreateMap<Product, UpdateProductDto>();
			CreateMap<Product, ReadProductDto>();
			CreateMap<CreateProductDto, Product>();
			CreateMap<UpdateProductDto, Product>();
			CreateMap<ReadProductDto, Product>();

			CreateMap<Category, CreateCategoryDto>();
			CreateMap<Category, UpdateCategoryDto>();
			CreateMap<Category, ReadCategoryDto>();
			CreateMap<CreateCategoryDto, Category>();
			CreateMap<UpdateCategoryDto, Category>();
			CreateMap<ReadCategoryDto, Category>();

			CreateMap<Order, CreateOrderDto>();
			CreateMap<CreateOrderDto, Order>();
			// CreateMap<Order, UpdateOrderDto>();
			CreateMap<Order, ReadOrderDto>();
			CreateMap<ReadOrderDto, Order>();

			CreateMap<OrderItem, OrderItemDto>();
			CreateMap<OrderItemDto, OrderItem>();

			CreateMap<ReadOrderDto, OrderItem>();
			CreateMap<OrderItem, ReadOrderItemDto>();

			CreateMap<OrderItemDto, Order>();
			CreateMap<Order, OrderItemDto>();
		}
	}
}
