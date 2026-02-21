using EcommearceBackend.Business.src.Dtos.Category;
using EcommearceBackend.Business.src.Dtos.Product;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface ICategoryService
	{
		Task<IEnumerable<ReadCategoryDto>> GetAllCategoriesAsync(QueryOptions queryOptions);
		Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto);
		Task<ReadCategoryDto> GetCategoryByIdAsync(int categoryId);
		Task<ReadCategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryDto categoryDto);
		Task<bool> DeleteCategoryAsync(int categoryId);
		Task<IEnumerable<ReadProductDto>> GetAllProductsInCategoryAsync(int categoryId);
	}
}
