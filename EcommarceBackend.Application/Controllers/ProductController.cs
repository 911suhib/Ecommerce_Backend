using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.src.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController:ControllerBase
	{
	
	   private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]

		public async Task<ActionResult<ReadProductDto>> GetAllProductsAsync([FromQuery] QueryOptions queryOptions)
		{
			var products= await _productService.GetAllProductsAsync(queryOptions);	
			return Ok(products);
		}

		[HttpGet("id")]
		public async Task<ActionResult<ReadProductDto>>GetProductByIdAsync(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product == null)
				return NotFound();
			return Ok(product);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<ReadProductDto>> CreatProductAsync([FromBody] CreateProductDto product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var newProduct = await _productService.CreateProductAsync(product);
			return Ok(newProduct);
		}
		[HttpPut("id")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<ReadProductDto>> UpdateProductAsync(int id, [FromBody]UpdateProductDto productdto)
		{
			var product = await _productService.UpdateProductAsync(id, productdto);
			return Ok(product);
		}

		[HttpDelete("id")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<bool>> DeleteProductByIdAsync(int id)
		{
			var result =await _productService.DeleteProductByIdAsync(id);	
			if (result == false)
			{
				return NotFound();

			}
			return Ok(result);
		}
	}
}
