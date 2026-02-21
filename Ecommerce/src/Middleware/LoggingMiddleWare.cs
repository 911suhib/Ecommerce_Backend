
namespace EcommerceBackend.Framework.src.Middleware
{
	public class LoggingMiddleWare : IMiddleware
	{
		private readonly ILogger<LoggingMiddleWare> _logger;
		public LoggingMiddleWare(ILogger<LoggingMiddleWare> logger)
		{
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var originalResponseBody = context.Response.Body;

			using var responseBodyStream = new MemoryStream();
			context.Response.Body = responseBodyStream;

			try
			{
				_logger.LogInformation(
					"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} - Request:{Method} {Path}{QueryString}, User-Agent: {UserAgent}, Remote IP: {RemoteIpAddress}",
					DateTime.Now,
					context.Request.Method,
					context.Request.Path,
					context.Request.QueryString,
					context.Request.Headers["User-Agent"],
					context.Connection.RemoteIpAddress
				);

				await next(context);

				responseBodyStream.Seek(0, SeekOrigin.Begin);

				var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

				_logger.LogInformation("Response: {StatusCode}, Content: {ResponseBody}",
					context.Response.StatusCode,
					responseBody);

				responseBodyStream.Seek(0, SeekOrigin.Begin);
				await responseBodyStream.CopyToAsync(originalResponseBody);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while processing the request.");
				throw;
			}
			finally
			{
				context.Response.Body = originalResponseBody; // ✅ الحل
			}
		}

	}
}
