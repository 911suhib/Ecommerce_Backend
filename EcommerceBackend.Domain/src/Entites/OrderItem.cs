namespace EcommerceBackend.Domain.Entities
{
	// OrderItem
	public class OrderItem : BaseEntity
    {
        // Foreign Key & Navigation
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
