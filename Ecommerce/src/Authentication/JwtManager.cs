using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Framework.src.Database;
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
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		private readonly AppDbContext _dbcontext;
		public JwtManager(IOptions<JwtOptions> options, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository,AppDbContext dbContext)
		{

			_options = options.Value;	
			_userRepository = userRepository;
			_refreshTokenRepository = refreshTokenRepository;
			_dbcontext = dbContext;
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
				Expires = DateTime.UtcNow.AddMinutes(2),
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = signingCredentials
			};
			var token=new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);
			string tokenValue=new JwtSecurityTokenHandler().WriteToken(token);
			return tokenValue;
		}

		public async Task<string> RefreshAccessToken(string refreshToken)
		{
		 var token=await _refreshTokenRepository.GetByTokenAsync(refreshToken);
			if (token == null || token.IsRevoked)
				throw new SecurityTokenException("Invalid refresh Token");

			if (token.ExpiryDate < DateTime.UtcNow)
				throw new SecurityTokenException("Refresh token expired");

			token.IsRevoked = true;
		    await _dbcontext.SaveChangesAsync();
			var user =await _userRepository.GetByIdAsync(token.UserId);

			return  GenerateAccessToken(user);

		}
		public async Task RevokeAsync(string refreshToken)
		{
			 await _refreshTokenRepository.RevokeAsync(refreshToken);	
		}

	}
}
