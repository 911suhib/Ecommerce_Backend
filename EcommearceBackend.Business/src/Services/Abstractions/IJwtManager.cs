using EcommerceBackend.Domain.Entities;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IJwtManager
	{
		public string GenerateAccessToken(User user);
		public Task<string> RefreshAccessToken(string refreshToken);
		public Task RevokeAsync(int  id);
		public string GenerateTempToken(string email);


	}
}
