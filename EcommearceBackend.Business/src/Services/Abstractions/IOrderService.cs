using EcommearceBackend.Business.src.Dtos.Order;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IOrderService
	{
		Task<IEnumerable<ReadOrderDto>> GetAllOrdersAsync(QueryOptions queryOptions);
		Task<IEnumerable<ReadOrderDto>> GetOrdersWithDetailsdAsync();
		Task<ReadOrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
		// Task<ReadOrderDto> UpdateOrderAsync(int OrderId, UpdateOrderDto updateOrderDto);
		Task<ReadOrderDto> GetOrderByIdAsync(int orderId);
		Task<bool> DeleteOrderByIdAsync(int orderId);
		Task<IEnumerable<ReadOrderDto>> GetOrdersByUserIdAsync(int userId);
	}
}
