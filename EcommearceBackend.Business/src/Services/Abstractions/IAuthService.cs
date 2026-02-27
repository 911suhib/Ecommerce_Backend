using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IAuthService
	{
		Task<ReadUserDto> CreateUserAsync(CreateUserDto userDto);

		Task<ReadUserDto> CreateAdminAsync(CreateUserDto userDto);

		 Task<string> VerifyEmail(string email, string code);

		Task<string> AutheticateUser(UserCredentialsDto userCredentials);
		Task<string> RefreshToken(string refreshToken);

		Task<bool> ForgetBassword(string email);

		 Task<bool> CheckCodeHashed(string email, string code);

		Task<User> ResetPasswordHashed(string email, string code, string newPassword);

		}
}
