using EcommerceBackend.Domain.Entities;

namespace EcommerceBackend.Domain.src.Abstractions
{
	public interface ICategoryRepository:IBaseRepository<Category>	{
		Task<Category> GetCategoryByNameAsync(string categoryName);
		Task<IEnumerable<Product>> GetAllProductsInCategoryAsync(int id);
	}
}
