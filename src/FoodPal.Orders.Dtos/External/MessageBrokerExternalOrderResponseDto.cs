using System.Collections.Generic;

namespace FoodPal.Orders.Dtos.External
{
	public class MessageBrokerExternalOrderResponseDto
	{
		public int OrderId { get; set; }
		public List<int> OrderItemIds { get; set; }
	}
}
