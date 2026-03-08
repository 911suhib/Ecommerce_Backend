using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.src.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]

	public class ProductController : ControllerBase
	{
		private readonly IWebHostEnvironment _env; // إضافة البيئة

		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment env)
		{
			_productService = productService;
			_categoryService = categoryService;
			_env = env;
		}

		[HttpGet]

		public async Task<ActionResult<ReadProductDto>> GetAllProductsAsync([FromQuery] QueryOptions queryOptions)
		{
			var products = await _productService.GetAllProductsAsync(queryOptions);
			return Ok(products);
		}

		[HttpGet("id")]
		public async Task<ActionResult<ReadProductDto>> GetProductByIdAsync(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product == null)
				return NotFound();
			return Ok(product);
		}

		[HttpPost]

		public async Task<ActionResult<ReadProductDto>> CreatProductAsync([FromBody] CreateProductDto product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var newProduct = await _productService.CreateProductAsync(product);
			return Ok(newProduct);
		}
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<ReadProductDto>> UpdateProductAsync(int id, [FromBody] UpdateProductDto productdto)
		{
			var product = await _productService.UpdateProductAsync(id, productdto);
			return Ok(product);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<bool>> DeleteProductByIdAsync(int id)
		{
			var result = await _productService.DeleteProductByIdAsync(id);
			if (result == false)
			{
				return NotFound();

			}
			return Ok(result);
		}
		[HttpPost("upload-image")]
		public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int Category)
		{
			try
			{
				if (file == null || file.Length == 0)
					return BadRequest(new { message = "No file uploaded" });

				// 1. فحص القسم والحصول على الاسم
				var categoryName = await _categoryService.GetCategoryName(Category);
				if (string.IsNullOrEmpty(categoryName))
				{
					return BadRequest(new { message = $"Category with ID {Category} not found." });
				}

				// تنظيف اسم المجلد من المسافات لضمان عدم حدوث خطأ في المسار
				categoryName = categoryName.Replace(" ", "_");

				// 2. بناء المسار باستخدام WebRootPath (هذا هو التعديل المنقذ)
				// في اللوكال رح يروح لـ wwwroot وفي السيرفر رح يروح للمكان الصحيح
				var rootPath = _env.WebRootPath;

				// إذا كان WebRootPath نل (أحياناً في بعض السيرفرات)، نستخدم المجلد الحالي كحل احتياطي
				if (string.IsNullOrEmpty(rootPath))
				{
					rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
				}

				var uploadsFolder = Path.Combine(rootPath, "images", categoryName);

				// التأكد من وجود المجلدات (أو إنشاؤها)
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				// 3. إنشاء اسم فريد للملف
				var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);

				// 4. حفظ الملف
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				// 5. إرجاع رابط الصورة (ليستخدمه React في السورس src)
				// الرابط سيكون مثلاً: /images/Electronics/abc-123.jpg
				var relativePath = $"/images/{categoryName}/{uniqueFileName}";

				return Ok(new { imageUrl = relativePath });
			}
			catch (Exception ex)
			{
				// إذا ضرب 500، رح تعرف السبب من الـ error message في الـ Swagger
				return StatusCode(500, new
				{
					message = "Internal Server Error during upload",
					error = ex.Message,
					innerError = ex.InnerException?.Message
				});
			}
		}
	}


}
