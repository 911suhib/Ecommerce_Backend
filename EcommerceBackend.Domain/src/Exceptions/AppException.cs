using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceBackend.Domain.src.Exceptions
{
	public abstract class AppException:Exception
	{
		public object? ExtraData { get; }

		public int StatusCode { get; }

		protected AppException(string message, int statusCode,object? extraData=null) : base(message)
		{
			StatusCode = statusCode;
			ExtraData = extraData;
		}
	}
}
