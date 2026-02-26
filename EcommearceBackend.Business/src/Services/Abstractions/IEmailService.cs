using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface IEmailService
	{
		Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage);
	}
}
