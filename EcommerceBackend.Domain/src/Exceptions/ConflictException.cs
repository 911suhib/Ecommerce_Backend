using System.Net;

namespace EcommerceBackend.Domain.src.Exceptions
{
	public class ConflictException:AppException
	{
		public ConflictException(string message):base(message,(int)HttpStatusCode.Conflict)
		{
			
		}
	}
}
