using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceBackend.Framework.src.Authentication
{
	public class JwtManager : IJwtManager
	{

		private readonly JwtOptions _options;
		private readonly IUserRepository _userRepository;

		public JwtManager(IOptions<JwtOptions> options)
		{
			_options = options.Value;	
		}


		public string GenerateAccessToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new Claim (ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.Role,user.Role.ToString())
			};
			var securitKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
			var signingCredentials=new SigningCredentials(securitKey,SecurityAlgorithms.HmacSha256);


			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = _options.Issuer,
				Audience = _options.Audience,
				Expires = DateTime.UtcNow.AddDays(1),
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = signingCredentials
			};
			var token=new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);
			string tokenValue=new JwtSecurityTokenHandler().WriteToken(token);
			return tokenValue;
		}

		public Task<string> RefreshAccessToken(string refreshToken)
		{
			throw new NotImplementedException();
		}
	}
}
