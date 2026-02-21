using System.Text.Json.Serialization;

namespace EcommerceBackend.Domain.src.Entites
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum OrderStatus
	{
		Pending,
		Processing,
		Shipped,
		Delivered,
		Cancelled
	}
}