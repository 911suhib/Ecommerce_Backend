using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Framework.src.Repositories
{
	public class OrderRepository : BaseRepository<Order>, IOrderRepository
	{
		private readonly AppDbContext _applicationDbContext;
		private readonly DbSet<Order> _dbSet;	
		public OrderRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
			_dbSet = _applicationDbContext.Set<Order>();
		}

		public async Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync()
		{
			return await _dbSet.AsNoTracking().Include(order=>order.OrderItems).ThenInclude(OrderItem=>OrderItem.Product).ToListAsync();
		}

		public async Task<Order> GetByIdWithOtherDetailsAsync(int orderId)
		{
			return await _dbSet.AsNoTracking().Include(order => order.OrderItems).ThenInclude(OrderItem => OrderItem.Product).FirstOrDefaultAsync(order => order.Id == orderId);
		}

		public async Task<IEnumerable<Order>> GetOrdersForUserAsync(int userId)
		{
			return await _dbSet.AsNoTracking().Where(_dbSet => _dbSet.UserId == userId).Include(order => order.OrderItems).ThenInclude(OrderItem => OrderItem.Product).ToListAsync();
		}
	}
}
