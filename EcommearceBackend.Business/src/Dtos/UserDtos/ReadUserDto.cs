using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.UserDtos
{
	public  class ReadUserDto
	{
		public  string FName { get; set; }
		public  string LName { get; set; }
		public  string Email { get; set; }
        public string Phone { get; set; }
		public string Role { get; set; }
	}
}
