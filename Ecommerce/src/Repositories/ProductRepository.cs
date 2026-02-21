using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Common;
using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace EcommerceBackend.Framework.src.Repositories
{
	public class ProductRepository : BaseRepository<Product>, IProductRepository
	{
		private readonly DbSet<Product>_product;
		private readonly AppDbContext _applicationDbContext;
		public ProductRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;	

			_product = _applicationDbContext.Set<Product>();
		}
		public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(QueryOptions queryOptions)
		{
			IQueryable<Product> query = _product
				.Include(p => p.Category); 

			if (!string.IsNullOrEmpty(queryOptions.SearchKeyword))
			{
				query = query.Where(p =>
					p.Name.ToLower().Contains(queryOptions.SearchKeyword.ToLower()));
			}

			var sortBy = string.IsNullOrEmpty(queryOptions.SortBy)
				? "Id"
				: queryOptions.SortBy;

			var orderBy = $"{sortBy} {(queryOptions.SortDescending ? "desc" : "asc")}";

			query = query.OrderBy(orderBy)
						 .Skip((queryOptions.PageNumber - 1) * queryOptions.PageSize)
						 .Take(queryOptions.PageSize);

			return await query.AsNoTracking().ToListAsync();
		}

	}
}
