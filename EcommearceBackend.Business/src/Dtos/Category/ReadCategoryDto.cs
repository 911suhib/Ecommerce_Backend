using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.Category
{
	public class ReadCategoryDto
	{
		public int Id { get; set; }
		public required string Name { get; set; }
	}
}
