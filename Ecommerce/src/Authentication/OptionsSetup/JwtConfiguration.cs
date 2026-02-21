using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace EcommerceBackend.Framework.src.Authentication.OptionsSetup
{
	public static class JwtConfiguration
	{
		public static void ConfigureJwt(IServiceCollection services,IConfiguration configuration)
		{
			services.Configure<JwtOptions>(
				configuration.GetSection("JwtOptions")
				);


			services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer();

			services.AddAuthorization();

		}
	}
}
