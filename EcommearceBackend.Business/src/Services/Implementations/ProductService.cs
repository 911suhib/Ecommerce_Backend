using AutoMapper;
using EcommearceBackend.Business.src.Dtos.Category;
using EcommearceBackend.Business.src.Dtos.Product;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Implementations
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		private readonly ISanitizerService _sanitizerService;
		private readonly ICategoryRepository _categoryRepository;
		public ProductService(IProductRepository productRepository, IMapper mapper, ISanitizerService sanitizerService, ICategoryRepository categoryRepository)
		{
			_productRepository= productRepository;
			_mapper= mapper;
			_sanitizerService= sanitizerService;
			_categoryRepository= categoryRepository;

		}

		public async Task<ReadProductDto> CreateProductAsync(CreateProductDto createProductDto)
		{
			try { 
		
				var sanitizedDto=_sanitizerService.SanitizeDto(createProductDto);
				if (sanitizedDto == null)
				{
					throw new ArgumentNullException(nameof(createProductDto));
				}

				var productDtoProperties=typeof(CreateProductDto).GetProperties();

				foreach(var property in productDtoProperties) {
				var dtoValue=property.GetValue(sanitizedDto);

					if (property.Name.ToLower() == "imageurl")
					{
						Console.WriteLine($"{property.Name} : value is {dtoValue}");
					}

					if (dtoValue is null or (object)"")
					{
						throw new ArgumentException($"{property.Name} is required.");
					}

				}

				var category=await _categoryRepository.GetByIdAsync(sanitizedDto.CategoryId);

				if (category == null) {
					throw new ArgumentException($"Category with ID {sanitizedDto.CategoryId} not found.");

				}
				var newProduct=_mapper.Map<Product>(sanitizedDto);
				newProduct= await _productRepository.AddAsync(newProduct);

				var readProductDto = _mapper.Map<ReadProductDto>(newProduct);
				readProductDto.Category = _mapper.Map<ReadCategoryDto>(category);
				return readProductDto;

			}


			catch(Exception ex) {

				Console.WriteLine("Mapping error: " + ex.Message);

				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner exception: " + ex.InnerException.Message);
				}
				throw;

			}
		}

		public async Task<bool> DeleteProductByIdAsync(int productId)
		{
			try {
				var existingProduct=await _productRepository.GetByIdAsync(productId);
				if (existingProduct == null) { 
				throw new ArgumentException($"Product with ID {productId} not found.");
				}
				return await _productRepository.DeleteByIdAsync(existingProduct.Id);
			}
			catch (Exception ex) {
				throw new Exception($"Error deleting product with ID {productId}: {ex.Message}", ex);
			}
		}

		public async Task<IEnumerable<ReadProductDto>> GetAllProductsAsync(QueryOptions queryOptions)
		{
			var products = await _productRepository.GetAllWithCategoryAsync(queryOptions);

			return _mapper.Map<IEnumerable<ReadProductDto>>(products);
		}


		public async Task<ReadProductDto> GetProductByIdAsync(int productId)
		{
			var product=await _productRepository.GetByIdAsync(productId)??
				throw new ArgumentException($"Product with ID {productId} not found.");
			var category= await _categoryRepository.GetByIdAsync(product.CategoryId);
			var readProductDto=_mapper.Map<ReadProductDto>(product);
			readProductDto.Category=_mapper.Map<ReadCategoryDto>(category);
			return readProductDto;
		}

		public async Task<ReadProductDto> UpdateProductAsync(int ProductId, UpdateProductDto updateProductDto)
		{

			try
			{

				var sanitizedDto=_sanitizerService.SanitizeDto(updateProductDto);

				var existingProduct=await _productRepository.GetByIdAsync(ProductId)??
					throw new ArgumentException($"Product with ID {ProductId} not found.");
				var existingProductDto=_mapper.Map<ReadProductDto>(existingProduct);

				var updatedProductDtoProperties=typeof(UpdateProductDto).GetProperties();

 
				foreach(var property in updatedProductDtoProperties) {
					 var dtoValue=property.GetValue(sanitizedDto);	
					if(dtoValue is null or (object)"") {
 						throw new ArgumentException($"{property.Name} is required.");
					}
					var product=existingProductDto.GetType().GetProperty(property.Name);
					product.SetValue(existingProductDto, dtoValue);
				}
				_mapper.Map(existingProductDto, existingProduct);
				var updatedProduct= await _productRepository.UpdateAsync(existingProduct.Id,existingProduct);	
				var category= await _categoryRepository.GetByIdAsync(updatedProduct.CategoryId);
				var readProductDto=_mapper.Map<ReadProductDto>(updatedProduct);
				readProductDto.Category=_mapper.Map<ReadCategoryDto>(category);
				return readProductDto;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
				}
				throw;
			}

}
	}
}
