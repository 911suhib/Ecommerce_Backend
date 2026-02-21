namespace EcommerceBackend.Domain.Entities
{
	// Product
	public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public  string ? Description { get; set; }
        public required decimal Price { get; set; }

        public required string ImgeUrl { get; set; }    
		// Foreign Key & Navigation
		public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }

		public int Inventory { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
