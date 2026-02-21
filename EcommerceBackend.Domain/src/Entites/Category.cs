namespace EcommerceBackend.Domain.Entities
{
	// Category
	public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
