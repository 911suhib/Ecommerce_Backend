using EcommearceBackend.Business.src.Dtos.Order;
using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;
		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ReadOrderDto>> GetAllOrdersAsync()
		{
			var orders = await _orderService.GetOrdersWithDetailsdAsync();
			return Ok(orders);
		}

		[HttpPost("Users/{userId}/Orders")]
		[Authorize(Roles = "Admin,Customer")]

		public async Task<ActionResult<ReadOrderDto>> CreatOrderAsync(int userId, [FromBody] CreateOrderDto orderDto)
		{
			var newOrder = await _orderService.CreateOrderAsync(userId, orderDto);
			return Ok(newOrder);
		}
		[HttpGet("{id}")]
		[Authorize(Roles = "Admin,Customer")]

		public async Task<ActionResult<ReadOrderDto>>GetOrderBIdAsync(int id)
		{
			var order=await _orderService.GetOrderByIdAsync(id); ;
			return Ok(order);
		}

		[HttpDelete("id")]
		public async Task<ActionResult<bool>> DeleteOrderAsync(int id) { 
		return Ok(await _orderService.DeleteOrderByIdAsync(id));
		}
		[HttpGet("User/{userId}")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<IEnumerable<ReadProductDto>>> GetOrderByUserIdAsync(int userId)
		{ 
		var userOrders=await _orderService.GetOrdersByUserIdAsync(userId);
			return Ok(userOrders);
		}
	}
}
