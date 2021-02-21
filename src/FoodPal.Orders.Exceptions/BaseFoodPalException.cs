using FoodPal.Orders.Dtos;
using System;

namespace FoodPal.Orders.Exceptions
{
	public class BaseFoodPalException : Exception
	{
		public ErrorInfoDto ErrorInfo { get; private set; }

		public BaseFoodPalException(ErrorInfoDto errorInfoDto) : base()
		{
			ErrorInfo = errorInfoDto;
		}
	}
}
