using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Dtos
{
	public class ErrorInfoDto
	{
		public ErrorInfoType Type { get; set; }

		public string Message { get; set; }

		public string Details { get; set; }
	}
}
