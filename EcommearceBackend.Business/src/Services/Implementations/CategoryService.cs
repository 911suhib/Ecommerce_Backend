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
	public class CategoryService : ICategoryService
	{
		private readonly ISanitizerService _sanitizerService;
		private readonly IMapper _mapper;
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository,ISanitizerService sanitizerService, IMapper mapper)
		{
		_categoryRepository= categoryRepository;
			_sanitizerService= sanitizerService;
			_mapper= mapper;
		}
		public async Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
		{
			try
			{

				var sanitzedDto = _sanitizerService.SanitizeDto(categoryDto);
				var existingCategory = await _categoryRepository.GetCategoryByNameAsync(sanitzedDto.Name);
				if (existingCategory != null)
				{
					throw new ArgumentException($"There is an existing category with the name `{sanitzedDto.Name}`. ");

				}
				var categoryDtoProperties = typeof(CreateCategoryDto).GetProperties();
				foreach (var property in categoryDtoProperties)
				{
					var dtoValue = property.GetValue(sanitzedDto);
					if (dtoValue is null or (object)"")
					{

						throw new ArgumentException($"{property.Name} is required");
					}
				}
				var newCategory = _mapper.Map<Category>(sanitzedDto);
				newCategory = await _categoryRepository.AddAsync(newCategory);
				var readCategoryDto = _mapper.Map<ReadCategoryDto>(newCategory);
				return readCategoryDto;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner exception: " + ex.InnerException.Message);
				}
				throw;
			}
		}
		public async Task<bool> DeleteCategoryAsync(int categoryId)
		{
			var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
			return await _categoryRepository.DeleteByIdAsync(categoryId);

		}

		public async Task<IEnumerable<ReadCategoryDto>> GetAllCategoriesAsync(QueryOptions queryOptions)
		{
			var categories = await _categoryRepository.GetAllAsync(queryOptions);	
			var readCategoryDtos = _mapper.Map<IEnumerable<ReadCategoryDto>>(categories);	
			return readCategoryDtos;
		}

		public async Task<IEnumerable<ReadProductDto>> GetAllProductsInCategoryAsync(int categoryId)
		{
			var category=await _categoryRepository.GetAllProductsInCategoryAsync(categoryId);
			var readProductDtos = _mapper.Map<IEnumerable<ReadProductDto>>(category);	

			return readProductDtos;

		}

		public async Task<ReadCategoryDto> GetCategoryByIdAsync(int categoryId)
		{
			var category = await _categoryRepository.GetByIdAsync(categoryId)
				?? throw new ArgumentException($"Category with ID {categoryId} not found.");
			var readCategoryDto = _mapper.Map<ReadCategoryDto>(category);	
			return readCategoryDto;

		}

		public async Task<string> GetCategoryName(int categoryId)
		{
			var name = await _categoryRepository.GetNameCategory(categoryId)??throw new ArgumentException($"Category not found");
		 return	name;
		}

		public async Task<ReadCategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryDto categoryDto)
		{
			var existingCategory = await _categoryRepository.GetByIdAsync(categoryId)
					?? throw new ArgumentException("Category with ID {categoryId} not found.");
			_mapper.Map(categoryDto, existingCategory);
			existingCategory.UpdatedAt = DateTime.UtcNow;
			existingCategory = await _categoryRepository.UpdateAsync(categoryId, existingCategory);

			return _mapper.Map<ReadCategoryDto>(existingCategory);

		}
	}
}
