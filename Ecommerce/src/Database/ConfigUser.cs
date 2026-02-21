using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigUser : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(p=>p.Id);
			builder.Property(p => p.FName).HasMaxLength(200).IsRequired();
			 builder.Property(p => p.LName).HasMaxLength(200).IsRequired();
			 builder.Property(p => p.PhoneNumber).HasMaxLength(15).IsRequired();
			 builder.HasIndex(p => p.Email).IsUnique();
			builder.Property(u => u.Email)
	  .HasMaxLength(255)
	  .IsRequired();

			builder.Property(p => p.HashedPassword).HasMaxLength(500).IsRequired();
			 builder.HasMany(p => p.Orders)
				 .WithOne(o => o.User)
				 .HasForeignKey(o => o.UserId);
			builder.Property(p=>p.Role).IsRequired();
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

		}
	}
}
