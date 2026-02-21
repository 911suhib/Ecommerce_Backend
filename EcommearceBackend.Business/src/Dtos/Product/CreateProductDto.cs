using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.Product
{
	public class CreateProductDto
	{
		public required string Name { get; set; }
		public  string? Description { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public int Inventory { get; set; }
		public required string ImgeUrl { get; set; }

		public int BrandID { get; set; }
	}
}
