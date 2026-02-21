using BCrypt.Net;

namespace EcommerceBackend.Business.src.Services.Common
{
	public class PasswordService
	{
 		public static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

 		public static bool VerifyPassword(string hashedPassword, string providedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
		}
	}
}