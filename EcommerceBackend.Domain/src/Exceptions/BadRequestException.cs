using System.Net;

namespace EcommerceBackend.Domain.src.Exceptions
{
	public class BadRequestException : AppException
	{
		public BadRequestException(string message) : base(message,(int)HttpStatusCode.BadRequest) { }
	}
}
