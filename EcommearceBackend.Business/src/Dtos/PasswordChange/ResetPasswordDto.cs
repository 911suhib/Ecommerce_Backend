using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.PasswordChange
{
	public class ResetPasswordDto
	{
		public string Email { get; set; }
		public string Code { get; set; }
		public string NewPassword { get; set; }
	}
}
