using EcommerceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceBackend.Domain.src.Entites
{
	public class UserRefreshToken
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string RefreshToken { get; set; }
		public bool IsRevoked { get; set; }	
		public DateTime ExpiryDate { get; set; }
		public User User { get; set; }

	}
}
