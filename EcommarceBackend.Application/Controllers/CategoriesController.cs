using EcommearceBackend.Business.src.Dtos.Category;
using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.src.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]

		public async Task<ActionResult<ReadCategoryDto>> GetAllCategoriesAsync([FromQuery] QueryOptions queryOptions)
		{
			var categories = await _categoryService.GetAllCategoriesAsync(queryOptions);
			return Ok(categories);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ReadCategoryDto>> CreateCategoryAsync([FromBody] CreateCategoryDto categoryDto)
		{
			var category = await _categoryService.CreateCategoryAsync(categoryDto);
			return Ok(category);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<ReadCategoryDto>> GetCategoryByIdAsync(int id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			if (category is null)
				return NotFound();
			return Ok(category);
		}
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ReadCategoryDto>> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryDto categoryDto)
		{
			var category = await _categoryService.UpdateCategoryAsync(id, categoryDto);
			return Ok(categoryDto);
		}
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<bool>> DeleteCategoryAsync(int id)
		{
			var result = await _categoryService.DeleteCategoryAsync(id);
			if (!result)
			{
				NotFound();
				return false;
			}
			return true;
		}
		[HttpGet("{id}/products")]
		public async Task<ActionResult<ReadProductDto>> GetAllProductInCategoryAsync(int id)
		{
			var products = await _categoryService.GetAllProductsInCategoryAsync(id);
			return Ok(products);
		}
	}
}
