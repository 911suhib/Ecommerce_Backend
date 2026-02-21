using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Business.src.Services.Common;
using EcommerceBackend.Domain.src.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Common
{
public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IJwtManager _jwtManager;

		public AuthService(IUserRepository userRepository, IJwtManager jwtManager)
		{
			_userRepository = userRepository;
			_jwtManager = jwtManager;
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

			return token;

		}

		public Task<string> RefreshToken(string refreshToken)
		{
			throw new NotImplementedException();
		}
	}
}
