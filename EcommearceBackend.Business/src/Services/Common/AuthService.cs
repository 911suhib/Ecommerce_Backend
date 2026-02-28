using AutoMapper;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommearceBackend.Business.src.Services.Implementations;
using EcommerceBackend.Business.src.Services.Common;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Common;
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
		private readonly ISanitizerService _sanitizerService;
		private readonly IMapper _mapper;
		public AuthService(IUserRepository userRepository,IMapper mapper,ISanitizerService sanitizerService,IEmailService emailService ,IJwtManager jwtManager,IRefreshTokenRepository refreshTokenRepository)
		{
			_userRepository = userRepository;
			_jwtManager = jwtManager;
			_refreshTokenRepository = refreshTokenRepository;
			_emailService = emailService;
			_sanitizerService = sanitizerService;
			_mapper = mapper;
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

			if (!BCrypt.Net.BCrypt.Verify(code,user.EmailVerificationCode))
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

		private async Task<User> Verification(User user)
		{
			byte[] randomNumber = new byte[6];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			int otpInt = Math.Abs(BitConverter.ToInt32(randomNumber, 0) % 1000000);
			string otp = otpInt.ToString("D6");

			string hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);

			user.EmailVerificationCode = hashedOtp;
			user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);
			user.IsEmailVerified = false;

			await _userRepository.UpdateAsync(user.Id, user);
			string subject = "رمز تفعيل الحساب";
			string htmlMessage = $@"
<div style='font-family: Arial; text-align:center;'>
    <h2>رمز التفعيل الخاص بك:</h2>
    <h1>{otp}</h1>
    <p>صالح لمدة 5 دقائق فقط</p>
</div>";
			var emailSent = await _emailService.SendEmailAsync(user.Email, subject, htmlMessage);
			if (!emailSent)
			{
				throw new ArgumentException("not found Email");
			}
			return user;

		}
		public async Task<ReadUserDto> CreateAdminAsync(CreateUserDto userDto)
		{
			try
			{
				var sanitizedDto = _sanitizerService.SanitizeDto(userDto);
				var existingUser = await _userRepository.GetUserByEmailAsync(sanitizedDto.Email);
				if (existingUser is not null)
				{
					if (!existingUser.IsEmailVerified)
					{
						await Verification(existingUser);
						throw new ArgumentException("Verification code resent. Please verify your email.");

					}

					throw new ArgumentException("A user with this email already exist.");
				}
				var userDtoProperties = typeof(CreateUserDto).GetProperties();
				foreach (var property in userDtoProperties)
				{
					var dtoValue = property.GetValue(userDto);
					if (dtoValue is null or (object)"")
					{
						throw new ArgumentException($"{property.Name} is required.");
					}
				}
				bool IsValidEmail = Validator.IsValidEmail(sanitizedDto.Email);

				if (!IsValidEmail)
				{
					throw new ArgumentException("Invalid Email address.");
				}



				var userEntity = _mapper.Map<User>(sanitizedDto);

				var hashedPassword = BCrypt.Net.BCrypt.HashPassword(sanitizedDto.Password);
				userEntity.HashedPassword = hashedPassword;

				userEntity = await _userRepository.CreateAdminAsync(userEntity);

				userEntity = await Verification(userEntity);

				var readUserDto = _mapper.Map<ReadUserDto>(userEntity);
				return readUserDto;

			}
			catch (Exception ex)
			{
				Console.WriteLine("Mapping error: " + ex.Message);

				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner exception: " + ex.InnerException.Message);
				}
				throw;
			}
		}
		public async Task<ReadUserDto> CreateUserAsync(CreateUserDto userDto)
		{
			try
			{
				var sanitizedDto = _sanitizerService.SanitizeDto(userDto);

				var existingUser = await _userRepository.GetUserByEmailAsync(sanitizedDto.Email);
				if (existingUser is not null)
				{
					if (!existingUser.IsEmailVerified)
					{
						await Verification(existingUser);
						throw new ArgumentException("Verification code resent. Please verify your email.");

					}

					throw new ArgumentException("A user with this email already exist.");
				}

				var userDtoProperties = typeof(CreateUserDto).GetProperties();

				foreach (var property in userDtoProperties)
				{
					var value = property.GetValue(sanitizedDto);
					if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
					{
						throw new ArgumentException($"Property {property.Name} cannot be empty or whitespace.");
					}
				}
				bool IsValidEmail = Validator.IsValidEmail(sanitizedDto.Email);

				if (!IsValidEmail)
				{
					throw new ArgumentException("Invalid Email address.");
				}
				var userEntity = _mapper.Map<User>(sanitizedDto);

				var hashedPassword = BCrypt.Net.BCrypt.HashPassword(sanitizedDto.Password);
				userEntity.HashedPassword = hashedPassword;
				userEntity.Role = UserRole.Customer;
				userEntity = await _userRepository.AddAsync(userEntity);

				userEntity = await Verification(userEntity);

				var readUserDto = _mapper.Map<ReadUserDto>(userEntity);
				return readUserDto;

			}
			catch (Exception ex)
			{
				Console.WriteLine("Mapping error: " + ex.Message);

				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner exception: " + ex.InnerException.Message);
				}
				throw;
			}
		}
		private async Task<User> VerificationPassword(User user)
		{
			byte[] randomNumber = new byte[6];
			using var rng=RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			
			int otpInt=Math.Abs(BitConverter.ToInt32(randomNumber, 0)%1000000);
			string otp = otpInt.ToString("D6");

			string hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);

			user.EmailVerificationCode = hashedOtp;
			user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);
 
			await _userRepository.UpdateAsync(user.Id, user);

			string subject = "رمز تغيير كلمة المرور";
			string htmlMessage = $@"
<div style='font-family: Arial; text-align:center;'>
    <h2>رمز التفعيل الخاص بك:</h2>
    <h1>{otp}</h1>
    <p>صالح لمدة 5 دقائق فقط</p>
</div>";
			var emailSent = await _emailService.SendEmailAsync(user.Email, subject, htmlMessage);
			if (!emailSent) throw new ArgumentException("Email not found");
			return user;

		}
		public async Task<bool> ForgetBassword(string email)
		{
			var userEmail = await _userRepository.GetUserByEmailAsync(email);
			if (userEmail is null)
			{
				throw new ArgumentException("this email is not found");
			}

			await VerificationPassword(userEmail);
			return true;

		}
		public async Task<bool> CheckCodeHashed(string email,string code) {
			var user = await _userRepository.GetUserByEmailAsync(email);
			if (user is null)
			{
				throw new ArgumentException("this email is not found");
			}
			if (user.VerificationCodeExpiry < DateTime.UtcNow)
				throw new ArgumentException("Code expired");

			if (!BCrypt.Net.BCrypt.Verify(code, user.EmailVerificationCode))
				throw new ArgumentException("Invalid code");
			return true;
		}

		public async Task<User> ResetPasswordHashed(string email, string code, string newPassword)
		{
			var user = await _userRepository.GetUserByEmailAsync(email);
			if (user is null) throw new ArgumentException("Email not found");

			if (user.VerificationCodeExpiry < DateTime.UtcNow)
				throw new ArgumentException("Code expired");

			if (!BCrypt.Net.BCrypt.Verify(code, user.EmailVerificationCode))
				throw new ArgumentException("Invalid code");
			user.HashedPassword=PasswordService.HashPassword(newPassword);

			user.EmailVerificationCode=null;
			user.VerificationCodeExpiry = null;
			await _userRepository.UpdateAsync(user.Id, user);
 			return user;
		}

	}
}
