using EcommerceBackend.Domain.src.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceBackend.Framework.src.Database
{
	public class ConfigRefreshToken : IEntityTypeConfiguration<UserRefreshToken>
	{
		public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id)
				.ValueGeneratedOnAdd()
				;
			builder.Property(x => x.UserId).IsRequired();
			builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens)
				.HasForeignKey(fk => fk.UserId)
				.OnDelete(DeleteBehavior.Cascade);
 		}
	}
}
