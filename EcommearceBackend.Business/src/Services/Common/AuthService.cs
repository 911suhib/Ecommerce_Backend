using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Business.src.Services.Common;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Entites;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Common
{
public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IJwtManager _jwtManager;
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		public AuthService(IUserRepository userRepository, IJwtManager jwtManager,IRefreshTokenRepository refreshTokenRepository)
		{
			_userRepository = userRepository;
			_jwtManager = jwtManager;
			_refreshTokenRepository = refreshTokenRepository;
		}


		public async Task<string> AutheticateUser(UserCredentialsDto userCredentials)
		{
			var user=await _userRepository.GetUserByEmailAsync(userCredentials.Email)
			?? throw new ArgumentException("Invalid login credentials.");

			var isAuthenticated = PasswordService.VerifyPassword(user.HashedPassword, userCredentials.Password);

			if (!isAuthenticated) {
				throw new ArgumentException("Invalid login credentials");
			}
			string token=_jwtManager.GenerateAccessToken(user);
			string refreshToken = GenerateRefreshToken();

			var userRefresh = new UserRefreshToken
			{
				UserId = user.Id,
				RefreshToken = refreshToken,
				ExpiryDate = DateTime.UtcNow.AddDays(7),
				IsRevoked = false
				
			};
			_refreshTokenRepository.AddRefreshToken(userRefresh);
			return token;

		}
		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
	 
		public async Task<string> RefreshToken(string refreshToken)
		{
			return await _jwtManager.RefreshAccessToken(refreshToken);
		}
	}
}
