
using System.Net;
using System.Text.Json;

namespace EcommerceBackend.Framework.src.Middleware
{
	public class ErrorHandlerMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				{
					if (ex.InnerException != null)
					{
						await HandleExceptionAysnc(context, ex.InnerException);	
					}
					await HandleExceptionAysnc(context, ex);
				}

			}
		}
		private static async Task HandleExceptionAysnc(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";	
			context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
			var errorResponse = new
			{
				Message = "An error occurred while processing your request",
				ExceptionMessage = exception.Message,
				ExceptionType = exception.GetType().FullName,
			};
			var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
			await context.Response.WriteAsync(jsonErrorResponse);
		}
	}
}
