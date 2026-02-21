using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigOrder: IEntityTypeConfiguration<Order>
	{
 
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasKey(o => o.Id);
			builder.HasOne(o => o.User)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(o => o.OrderItems)
				.WithOne(oi => oi.Order)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Property(o => o.TotalAmount)
				.HasColumnType("decimal(18,2)").IsRequired();
			builder.Property(o => o.Name).IsRequired().HasMaxLength(100);

			builder.Property(o => o.OrderDate)
				.HasDefaultValueSql("GETDATE()")
				.IsRequired();
			builder.Property(o => o.Status).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

		}
	}
}
