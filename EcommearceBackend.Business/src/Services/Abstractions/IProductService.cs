using EcommearceBackend.Business.src.Dtos.Product;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IProductService
	{
		Task<IEnumerable<ReadProductDto>> GetAllProductsAsync(QueryOptions queryOptions);
		Task <ReadProductDto> CreateProductAsync(CreateProductDto createProductDto);
		Task<ReadProductDto> UpdateProductAsync(int ProductId, UpdateProductDto updateProductDto);
		Task<ReadProductDto> GetProductByIdAsync(int productId);
		Task<bool> DeleteProductByIdAsync(int productId);
	}
}
