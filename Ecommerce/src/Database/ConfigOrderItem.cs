using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigOrderItem : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.HasKey(oi => oi.Id);
			builder.HasOne	(oi => oi.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(oi => oi.OrderId);
			builder.HasOne(oi => oi.Product).WithMany(builder => builder.OrderItems)
				.HasForeignKey(oi => oi.ProductId);
			builder.Property(oi => oi.Quantity).IsRequired();
			builder.Property(oi => oi.SubTotal).HasColumnType("").IsRequired();
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

		}
	}
}
