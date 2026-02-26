using EcommerceBackend.Domain.src.Entites;

namespace EcommerceBackend.Domain.Entities
{
	// Customer
	public class User : BaseEntity
    {
        public required string FName { get; set; }
        public required string LName { get; set; }
        public required string Email { get; set; }
		public required string PhoneNumber { get; set; }

		public required string HashedPassword { get; set; }

        public required UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

		public ICollection<UserRefreshToken> RefreshTokens { get; set; }
		public string? EmailVerificationCode { get; set; }
		public DateTime? VerificationCodeExpiry { get; set; }
		public bool IsEmailVerified { get; set; } = false;
	}
}
