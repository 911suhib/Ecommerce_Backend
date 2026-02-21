using EcommerceBackend.Domain.Entities;

namespace EcommerceBackend.Domain.src.Abstractions
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
		Task<IEnumerable<Order>>GetOrdersForUserAsync(int userId);
		Task<Order>GetByIdWithOtherDetailsAsync(int orderId);

		Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
	}
}
