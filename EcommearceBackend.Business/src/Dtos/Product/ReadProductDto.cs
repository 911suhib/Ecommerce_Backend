using EcommearceBackend.Business.src.Dtos.Category;
using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.Product
{
	public class ReadProductDto
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public decimal Price { get; set; }
		public  string? Description { get; set; }
		public string ImgeUrl { get; set; }
		public int Inventory { get; set; }
		public int CategoryId { get; set; }
		public ReadCategoryDto Category { get; set; }
	}
}
