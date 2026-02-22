using EcommerceBackend.Domain.src.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceBackend.Domain.src.Abstractions
{
	public interface IRefreshTokenRepository
	{
		public  Task RevokeAsync(string refreshToken);

		Task<string> AddRefreshToken(UserRefreshToken userRef);
 		Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);

		Task<UserRefreshToken> GetByTokenAsync(string refreshToken);
 	}
}
