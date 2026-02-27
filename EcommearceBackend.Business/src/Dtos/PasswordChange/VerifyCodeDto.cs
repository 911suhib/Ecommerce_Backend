using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.PasswordChange
{
	public class VerifyCodeDto
	{
		public string  Code { get; set; }
		public string Email { get; set; }
	}
}
