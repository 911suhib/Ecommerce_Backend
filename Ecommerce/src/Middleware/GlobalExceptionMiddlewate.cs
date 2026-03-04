using EcommerceBackend.Domain.src.Exceptions;

namespace EcommerceBackend.Framework.src.Middleware
{
	public class GlobalExceptionMiddlewate
	{
		private readonly RequestDelegate _next;

		public GlobalExceptionMiddlewate(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try{
				await _next(context);
			}
			catch (ConflictException ex)
			{
				context.Response.StatusCode = StatusCodes.Status409Conflict;
				await context.Response.WriteAsJsonAsync(new { message = ex.Message });
			}
			catch (NotFoundException ex)
			{
				context.Response.StatusCode = StatusCodes.Status404NotFound;
				await context.Response.WriteAsJsonAsync(new { message = ex.Message });
			}

			catch (BadRequestException ex)
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				await context.Response.WriteAsJsonAsync(new { message = ex.Message });
			}

			catch (Exception)
			{
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsJsonAsync(new { message = "Something went wrong" });
			}
		}

	}
}
