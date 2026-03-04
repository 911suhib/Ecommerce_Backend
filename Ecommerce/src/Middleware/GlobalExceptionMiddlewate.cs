using EcommerceBackend.Domain.src.Exceptions;

namespace EcommerceBackend.Framework.src.Middleware
{
	public class GlobalExceptionMiddlewate
	{
		private readonly RequestDelegate _next;
		private readonly IWebHostEnvironment _env;

		public GlobalExceptionMiddlewate(RequestDelegate next, IWebHostEnvironment env	)
		{
			_next = next;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (AppException ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = ex.StatusCode;

				var response = new Dictionary<string, object?>
				{
					{ "message", ex.Message }
				};

 				if (ex.ExtraData is not null)
				{
					foreach (var prop in ex.ExtraData.GetType().GetProperties())
					{
						response[prop.Name] = prop.GetValue(ex.ExtraData);
					}
				}

				await context.Response.WriteAsJsonAsync(
response					);

			}
			catch (Exception ex) {
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = 500;

				await context.Response.WriteAsJsonAsync(new
				{
					message = _env.IsDevelopment() ? ex.Message : "Something went wrong"
				});
			}
		}

	}
}
