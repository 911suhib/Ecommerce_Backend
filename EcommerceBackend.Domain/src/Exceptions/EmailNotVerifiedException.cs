using System.Net;

namespace EcommerceBackend.Domain.src.Exceptions
{
	public class EmailNotVerifiedException  

		: AppException
	{
		public EmailNotVerifiedException(string message) : base(message, (int)HttpStatusCode.Conflict, new {requiresVerification=true}) { }
	}
}
