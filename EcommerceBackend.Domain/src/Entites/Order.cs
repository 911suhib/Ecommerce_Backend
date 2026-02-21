using EcommerceBackend.Domain.src.Entites;

namespace EcommerceBackend.Domain.Entities
{
	// Order
	public class Order : BaseEntity
    {
        public required string Name { get; set; }
        public required decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Foreign Key & Navigation
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
