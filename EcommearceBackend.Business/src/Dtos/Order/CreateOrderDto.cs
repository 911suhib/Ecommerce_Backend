using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.Order
{
	public class CreateOrderDto
	{
 		public required List<OrderItemDto> Items { get; set; }



	}
}
