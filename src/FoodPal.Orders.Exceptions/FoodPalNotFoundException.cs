using FoodPal.Orders.Dtos;
using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Exceptions
{
	public class FoodPalNotFoundException : BaseFoodPalException
	{
		public FoodPalNotFoundException() : base(null) { }

		public FoodPalNotFoundException(string entityIdentifier) : this(new ErrorInfoDto() { Type = ErrorInfoType.Error, Message = entityIdentifier }) { }

		public FoodPalNotFoundException(ErrorInfoDto errorInfoDto) : base(errorInfoDto) { }
	}
}
