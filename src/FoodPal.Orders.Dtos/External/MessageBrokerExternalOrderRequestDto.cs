using System.Collections.Generic;

namespace FoodPal.Orders.Dtos.External
{
	public class MessageBrokerExternalOrderRequestDto
	{
		public int OrderId { get; set; }
		public List<MessageBrokerExternalOrderItemRequestDto> OrderItems { get; set; }
	}
}
