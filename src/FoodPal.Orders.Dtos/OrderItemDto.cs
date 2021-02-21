namespace FoodPal.Orders.Dtos
{
	public class OrderItemDto
	{
		public string Name { get; set; }

		public string ProviderId { get; set; }

		public short Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal TotalPrice => Quantity * Price;

		public StatusDto OrderItemStatus { get; set; }
	}
}
