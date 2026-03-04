using System.Net;

namespace EcommerceBackend.Domain.src.Exceptions
{
	public class NotFoundException : AppException
	{
		public NotFoundException(string message) : base(message,(int)HttpStatusCode.NotFound) { }
	}
}
