using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Entites;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Framework.src.Database
{
	public class AppDbContext:DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }	
		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }

		public DbSet<UserRefreshToken> UserRefreshToken { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
			
		}
		 protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(modelBuilder.GetType().Assembly);
		}
	}
}
