namespace FoodPal.Orders.Dtos
{
	public class NewOrderItemDto
	{
		public string Name { get; set; }

		public string ProviderId { get; set; }

		public short Quantity { get; set; }

		public decimal Price { get; set; }
	}
}
