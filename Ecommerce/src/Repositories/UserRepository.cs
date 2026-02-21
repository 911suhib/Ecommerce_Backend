using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Framework.src.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		private readonly AppDbContext ApplicationDbContext;
		private readonly DbSet<User> _User;

		public UserRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
		{
			ApplicationDbContext = applicationDbContext;
			_User = ApplicationDbContext.Set<User>();
		}


		public async Task<User> CreateAdminAsync(User user)
		{
			user.Role = UserRole.Admin;
			var entry=await _User.AddAsync(user);
			await ApplicationDbContext.SaveChangesAsync();
			return entry.Entity;	
		}

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			var user=await _User.AsNoTracking().FirstOrDefaultAsync(u=>u.Email==email);
			return user;
		}

		public async Task<User> UpdatePassword(string email, string PasswordHash)
		{
			      var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            user.HashedPassword = PasswordHash;
            await ApplicationDbContext.SaveChangesAsync();
            return user;
		}
	}
}
