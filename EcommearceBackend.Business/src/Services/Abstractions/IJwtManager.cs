using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IJwtManager
	{
		public string GenerateAccessToken(User user);
		Task<string> RefreshAccessToken(string refreshToken);
	}
}
