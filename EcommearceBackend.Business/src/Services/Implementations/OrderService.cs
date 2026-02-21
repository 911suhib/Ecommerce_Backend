using AutoMapper;
using EcommearceBackend.Business.src.Dtos.Order;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Common;
using EcommerceBackend.Domain.src.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Implementations
{
	public class OrderService : IOrderService
	{

		private readonly IOrderRepository _orderRepository;
		private readonly IUserRepository _userRepository;
		private readonly IProductRepository _productRepository;
		private readonly ISanitizerService _sanitizerService;
		private readonly IMapper _mapper;
		public OrderService(IOrderRepository orderRepository, IUserRepository userRepository,
		IProductRepository productRepository,
		ISanitizerService sanitizerService,
		IMapper mapper)
		{
			_orderRepository = orderRepository;
			_userRepository = userRepository;
			_productRepository = productRepository;
			_sanitizerService = sanitizerService;
			_mapper = mapper;
		}
		public async Task<ReadOrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
		{
			var user= await _userRepository.GetByIdAsync(userId);
			if (createOrderDto==null)
			{
				throw new ArgumentNullException(nameof(createOrderDto));
			}
			if (user == null)
			{
				throw new ArgumentException($"User with ID {userId} not found.");
			}

			var newOrder=_mapper.Map<Order>(createOrderDto);
			newOrder.UserId = userId;
			newOrder.OrderDate = DateTime.UtcNow;
			newOrder.Status = OrderStatus.Pending;
			newOrder.OrderItems= new List<OrderItem>();
			newOrder.Name = user.FName + " " + user.LName;
			foreach(var orderDto in createOrderDto.Items) {
			var orderItem=_mapper.Map<OrderItem>(orderDto);
			newOrder.OrderItems.Add(orderItem);
			}
			foreach (var orderItem in newOrder.OrderItems)
			{
				var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
				if (product == null)
				{
					throw new ArgumentException($"Product with ID {orderItem.ProductId} not found.");
				}
				orderItem.SubTotal = product.Price * orderItem.Quantity;
				if (product == null)
				{
					throw new ArgumentException($"Product with ID {orderItem.ProductId} not found.");
				}
				if (orderItem.Quantity > product.Inventory)
				{
					throw new ArgumentException($"Not enough quantity available in stock for product with ID {orderItem.ProductId}.");
				}
				product.Inventory -= orderItem.Quantity;
				orderItem.Product	= product;
				orderItem.SubTotal= product.Price * orderItem.Quantity;
			}
			newOrder.TotalAmount=newOrder.OrderItems.Sum(oi=>oi.SubTotal);
			await _orderRepository.AddAsync(newOrder);

			var readOrderDto =_mapper.Map<ReadOrderDto>(newOrder);
			readOrderDto.User = _mapper.Map<ReadUserDto>(user);


			return readOrderDto;

		}

		public async Task<bool> DeleteOrderByIdAsync(int orderId)
		{
			var existingOrder=await _orderRepository.GetByIdAsync(orderId);
			return await _orderRepository.DeleteByIdAsync(existingOrder.Id);
		}

		public Task<IEnumerable<ReadOrderDto>> GetAllOrdersAsync(QueryOptions queryOptions)
		{
			throw new NotImplementedException();
		}

		public async Task<ReadOrderDto> GetOrderByIdAsync(int orderId)
		{
			var existingOrder =await _orderRepository.GetByIdWithOtherDetailsAsync(orderId)
				?? throw new ArgumentException($"Order with ID {orderId} not found.");

			var user = await _userRepository.GetByIdAsync(existingOrder.UserId)
				?? throw new ArgumentException($"User with ID {existingOrder.UserId} not found.");

			var readOrderDto = _mapper.Map<ReadOrderDto>(existingOrder);
			readOrderDto.User = _mapper.Map<ReadUserDto>(user);
			readOrderDto.OrderItems= _mapper.Map<List<ReadOrderItemDto>>(existingOrder.OrderItems);
			return readOrderDto;
		}

		public async Task<IEnumerable<ReadOrderDto>> GetOrdersByUserIdAsync(int userId)
		{
			Console.WriteLine("User Id received in service layer 1.");
			var user = await _userRepository.GetByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentException($"User with ID {userId} not found.");
			}

			var userOrders = await _orderRepository.GetOrdersForUserAsync(user.Id);
			var readUserOrdersDto = new List<ReadOrderDto>();

			foreach (var order in userOrders)
			{
				var readOrderDto = _mapper.Map<ReadOrderDto>(order);
				readOrderDto.User = _mapper.Map<ReadUserDto>(user);
				readOrderDto.OrderItems = _mapper.Map<List<ReadOrderItemDto>>(order.OrderItems);
				readUserOrdersDto.Add(readOrderDto);
			}
			return readUserOrdersDto;
		}

		public async Task<IEnumerable<ReadOrderDto>> GetOrdersWithDetailsdAsync()
		{
			var orders = await _orderRepository.GetAllOrdersWithDetailsAsync();
			var readOrderDtos = new List<ReadOrderDto>();
			foreach (var order in orders)
			{
				var readOrderDto = _mapper.Map<ReadOrderDto>(order);
				var user = await _userRepository.GetByIdAsync(order.UserId);
				readOrderDto.User = _mapper.Map<ReadUserDto>(user);
				var orderItemsDto = new List<ReadOrderItemDto>();
				foreach (var item in order.OrderItems)
				{
					var product = await _productRepository.GetByIdAsync(item.ProductId);
					if (product != null)
					{
						var readOrderItemDto = _mapper.Map<ReadOrderItemDto>(item);
						orderItemsDto.Add(readOrderItemDto);
					}
				}
				readOrderDto.OrderItems = orderItemsDto;
				readOrderDtos.Add(readOrderDto);
			}
			return readOrderDtos;
		}
	}
}
