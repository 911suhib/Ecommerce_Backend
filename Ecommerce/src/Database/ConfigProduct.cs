using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigProduct : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasKey(b=>b.Id);

			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.Property(b=>b.Name)
				.IsRequired()
				.HasMaxLength(100);
			builder.Property(b=>b.Description)
				.HasMaxLength(500);

			builder.Property(b=>b.Price)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

			builder.Property(b=>b.Inventory)
				.IsRequired();

			builder.Property(b=>b.ImgeUrl)
				.IsRequired()
				.HasMaxLength(300);


			builder.HasOne(p => p.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryId);
			builder.HasOne(p => p.Brand)
				.WithMany(b => b.Products)
				.HasForeignKey(p => p.BrandId);

		}
	}
}
