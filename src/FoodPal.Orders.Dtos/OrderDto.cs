using System;
using System.Collections.Generic;

namespace FoodPal.Orders.Dtos
{
	public class OrderDto
	{
		public string CustomerId { get; set; }

		public string CustomerName { get; set; }

		public string CustomerEmail { get; set; }

		public IEnumerable<OrderItemDto> Items { get; set; }

		public DeliveryDetailsDto DeliveryDetails { get; set; }

		public StatusDto OrderStatus { get; set; }

		public string Comments { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime LastUpdatedAt { get; set; }
	}
}
