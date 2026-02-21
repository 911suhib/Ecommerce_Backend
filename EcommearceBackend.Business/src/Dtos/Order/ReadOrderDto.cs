using EcommearceBackend.Business.src.Dtos.UserDtos;

namespace EcommearceBackend.Business.src.Dtos.Order
{
	public class ReadOrderDto
	{
		public int Id { get; set; }
		public ReadUserDto User { get; set; }
		public DateTime OrderDate { get; set; }
		public List<ReadOrderItemDto> OrderItems { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
