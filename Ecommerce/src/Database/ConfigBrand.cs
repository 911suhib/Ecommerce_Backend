using EcommerceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigBrand : IEntityTypeConfiguration<Brand>
	{
		public void Configure(EntityTypeBuilder<Brand> builder)
		{
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.Name).IsRequired();
			builder.Property(x=>x.Id).ValueGeneratedOnAdd();	
		}
	}
}
