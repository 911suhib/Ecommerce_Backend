using EcommerceBackend.Domain.Entities;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IJwtService
	{
		public string GenerateAccessToken(User user);
		Task<string> RefreshAccessToken(string refreshToken);
	}
}
