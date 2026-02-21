namespace EcommerceBackend.Domain.Entities
{
	public class Brand : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
