 using EcommearceBackend.Business.src.Services.Abstractions;
using MailKit.Net.Smtp;      // SmtpClient الصحيح
using MailKit.Security;       // SecureSocketOptions
using Microsoft.EntityFrameworkCore.Query;
using MimeKit;                // MimeMessage 
namespace EcommearceBackend.Business.src.Services.Implementations
{
	public class EmailService : IEmailService
	{
		private readonly string _smtpServer = "smtp.gmail.com";
		private readonly int _smtpPort = 465;
		private readonly string _fromEmail = "suhibalkhaldy@gmail.com";
		private readonly string _password = "edlraagsjhfwjbsd";
 	 
		public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage)
		{
			try
			{
				var email = new MimeMessage();
				email.From.Add(new MailboxAddress("Ecommarce", _fromEmail));

				email.To.Add(new MailboxAddress("", toEmail));

				email.Subject = subject;
				email.Body = new TextPart("html")
				{
					Text = htmlMessage
				};
				using var smtp = new SmtpClient();
				await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.SslOnConnect);
				await smtp.AuthenticateAsync(_fromEmail, _password);
				await smtp.SendAsync(email);
				await smtp.DisconnectAsync(true);
				return true;
			}
			catch
			{
				return false;
			}
		}

	 
	}
}
