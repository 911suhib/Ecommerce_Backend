namespace EcommearceBackend.Business.src.Dtos.Order
{
	public class ReadOrderItemDto
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal SubTotal { get; set; }
	}
}
