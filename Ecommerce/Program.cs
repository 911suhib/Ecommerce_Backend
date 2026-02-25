using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommearceBackend.Business.src.Services.Common;
using EcommearceBackend.Business.src.Services.Implementations;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Framework.src.Authentication;
 using EcommerceBackend.Framework.src.Database;
using EcommerceBackend.Framework.src.Middleware;
using EcommerceBackend.Framework.src.Repositories;
using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

 // Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ErrorHandlerMiddleware>();
builder.Services.AddTransient<LoggingMiddleWare>();

 
builder.Services.AddControllers();


builder.Services.AddScoped<HtmlSanitizer>();        
 builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJwtManager, JwtManager>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var jwtSection = builder.Configuration.GetSection("JwtOptions");
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(jwtSection["SecretKey"])
			)
		};
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine("JWT Auth failed: " + context.Exception.Message);
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddCors(option =>
{
	option.AddPolicy("AllowAllFrontends", policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddAuthorization(
	option=>
	{
		option.AddPolicy("OrderOwnerOrAdmin", policy =>
		policy.RequireAssertion(context =>
		{
			var role=context.User.FindFirst(ClaimTypes.Role)?.Value;

			if (role == "Admin")
				return true;

			var userIdFromToken=context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var httpContext = context.Resource as HttpContext;

			var userIdFromRout = httpContext?.Request.RouteValues["id"]?.ToString();
			return userIdFromToken == userIdFromRout;
		})
		);
		
		
	}

	);


builder.Services.AddScoped<ISanitizerService, SanitizerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
 



builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();
app.UseCors("AllowAllFrontends");
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

}
app.UseMiddleware<LoggingMiddleWare>();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();

app.Run();
