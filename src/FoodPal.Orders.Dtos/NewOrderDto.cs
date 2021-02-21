using System.Collections.Generic;

namespace FoodPal.Orders.Dtos
{
	public class NewOrderDto
	{
		public string CustomerId { get; set; }

		public string CustomerName { get; set; }

		public string CustomerEmail { get; set; }

		public string Comments { get; set; }

		public List<NewOrderItemDto> Items { get; set; }

		public DeliveryDetailsDto DeliveryDetails { get; set; }
	}
}
