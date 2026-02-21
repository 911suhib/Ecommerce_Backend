using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Common;

namespace EcommerceBackend.Domain.src.Abstractions
{
	public interface IProductRepository : IBaseRepository<Product>
	{
		Task<IEnumerable<Product>> GetAllWithCategoryAsync(QueryOptions queryOptions);
	}
}
