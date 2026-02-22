using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Entites;
using EcommerceBackend.Framework.src.Database;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Framework.src.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly AppDbContext _dbContext;
		private readonly DbSet<UserRefreshToken> _userRefreshTokens;
		public RefreshTokenRepository(AppDbContext ApplicationDbContext)
		{
			_dbContext = ApplicationDbContext;
			_userRefreshTokens = _dbContext.Set<UserRefreshToken>();
		}

		public async Task<string> AddRefreshToken(UserRefreshToken userRef)
		{
 			await	_userRefreshTokens.AddAsync(userRef);
			await _dbContext.SaveChangesAsync();
			return userRef.RefreshToken;
		}

		public async Task<UserRefreshToken> GetByTokenAsync(string refreshToken)
		{
			var Token= await _userRefreshTokens.Where(x=>x.RefreshToken==refreshToken).FirstOrDefaultAsync();
			return Token;
		}

		public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
		{
			var userRefreshToken = await _userRefreshTokens.FirstOrDefaultAsync(urt => urt.UserId == userId && urt.RefreshToken == refreshToken);
			if (userRefreshToken == null) { 
			return false;
			}
			return true;
		}
		public async Task RevokeAsync(string refreshToken)
		{
			var token = await _userRefreshTokens.FirstOrDefaultAsync(x=>x.RefreshToken==refreshToken);

			if (token != null)
			{
				token.IsRevoked = true;
 			}
			await _dbContext.SaveChangesAsync();
		}
	}
}
