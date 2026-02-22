using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IJwtManager
	{
		public string GenerateAccessToken(User user);
		public Task<string> RefreshAccessToken(string refreshToken);
		public Task RevokeAsync(string refreshToken);


	}
}
