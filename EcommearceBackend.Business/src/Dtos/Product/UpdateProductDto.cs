using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.Product
{
	public class UpdateProductDto
	{
		public string? Name { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public int CategoryId { get; set; }
		public int Inventory { get; set; }
		public string ImgeUrl { get; set; }
	}
}
