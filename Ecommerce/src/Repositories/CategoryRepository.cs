using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Framework.src.Repositories
{
	public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
	{
		private readonly AppDbContext _applicationDbContext;
		private readonly DbSet<Category> _categories;
		public CategoryRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
			_categories = _applicationDbContext.Set<Category>();
		}

		public async Task<IEnumerable<Product>> GetAllProductsInCategoryAsync(int id)
		{
			var category=await _categories.AsNoTracking().Include(_categories=>_categories.Products).FirstOrDefaultAsync(c=>c.Id==id);
			if(category is null)
			{
				return null;
			}
			return category.Products;
		}

		public async Task<Category> GetCategoryByNameAsync(string categoryName)
		{
			return await _categories.AsNoTracking().FirstOrDefaultAsync(c=>c.Name.ToLower()==categoryName.ToLower());
		}

		public async Task<string> GetNameCategory(int id)
		{
			var category = await _categories.FindAsync(id);
			if (category == null)
				return null;
			return category.Name;
		}
	}
}
