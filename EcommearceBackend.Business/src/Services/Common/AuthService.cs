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
		private readonly IEmailService _emailService;
		public AuthService(IUserRepository userRepository,IEmailService emailService ,IJwtManager jwtManager,IRefreshTokenRepository refreshTokenRepository)
		{
			_userRepository = userRepository;
			_jwtManager = jwtManager;
			_refreshTokenRepository = refreshTokenRepository;
			_emailService = emailService;
		}


		public async Task<string> AutheticateUser(UserCredentialsDto userCredentials)
		{
			var user=await _userRepository.GetUserByEmailAsync(userCredentials.Email)
			?? throw new ArgumentException("Invalid login credentials.");
			if (!user.IsEmailVerified)
			{
				throw new ArgumentException("The email is not verified");
			}

			var isAuthenticated = PasswordService.VerifyPassword(user.HashedPassword, userCredentials.Password);

			if (!isAuthenticated) {
				throw new ArgumentException("Invalid login credentials");
			}
			var subject = "مرحبا بك مجدداََ";
			var html = $@"<h1>Ecommarce_Welcome </h1>    <div style='color:blue;text-align:center;'><h3> welcome back {user.FName}   </h3> </div>";
			var sendeEmail = await _emailService.SendEmailAsync(user.Email, subject, html);
			 
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
		public async Task<string> VerifyEmail(string email,string code)
		{
			var user =await _userRepository.GetUserByEmailAsync(email);
			if (user is null)
				throw new ArgumentException("User not found");

			if (user.EmailVerificationCode != code)
				throw new ArgumentException("Invalid code");

			if (user.VerificationCodeExpiry < DateTime.UtcNow)
				throw new ArgumentException("Code expired");
			user.IsEmailVerified = true;
			user.EmailVerificationCode = null;
			user.VerificationCodeExpiry = null;
			await _userRepository.UpdateAsync(user.Id,user);
			return "Email verified successfully";

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
