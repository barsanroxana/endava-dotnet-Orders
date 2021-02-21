namespace FoodPal.Orders.Dtos.External
{
	public class MessageBrokerExternalOrderItemRequestDto
	{
		public int OrderItemId { get; set; }
		public string Name { get; set; }
		public short Quantity { get; set; }
	}
}
