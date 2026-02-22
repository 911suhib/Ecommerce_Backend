using AutoMapper;
using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Dtos.UserDtos
{
	[AutoMap(typeof(User))]
	public class CreateUserDto
	{
		public  string FName { get; set; }
		public  string LName { get; set; }
		public  string Email { get; set; }
		public string Password { get; set; }
		public UserRole Role { get; set; }
		public string PhoneNumber { get; set; }
	}
}
