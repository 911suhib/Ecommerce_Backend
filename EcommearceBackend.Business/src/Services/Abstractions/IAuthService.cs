using EcommearceBackend.Business.src.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IAuthService
	{
		 Task<string> VerifyEmail(string email, string code);

		Task<string> AutheticateUser(UserCredentialsDto userCredentials);
		Task<string> RefreshToken(string refreshToken);
	}
}
