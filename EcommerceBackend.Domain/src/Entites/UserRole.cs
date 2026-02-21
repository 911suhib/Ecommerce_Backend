using System.Text.Json.Serialization;

namespace EcommerceBackend.Domain.Entities
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum UserRole
	{
		Customer,
		Admin
	}
}
