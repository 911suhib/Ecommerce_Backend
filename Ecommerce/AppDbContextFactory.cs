using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EcommerceBackend.Framework
{
	public class AppDbContextFactory
	   : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

			optionsBuilder.UseSqlServer(
				"Server=.\\SQLEXPRESS;Database=Ecommerce;Trusted_Connection=True;TrustServerCertificate=True");

			return new AppDbContext(optionsBuilder.Options);
		}
	}
}
