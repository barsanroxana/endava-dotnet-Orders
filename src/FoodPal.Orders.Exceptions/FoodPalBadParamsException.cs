using FoodPal.Orders.Dtos;
using FoodPal.Orders.Enums;
using System;
using System.Collections.Generic;

namespace FoodPal.Orders.Exceptions
{
	public class FoodPalBadParamsException : BaseFoodPalException
	{
		protected List<Exception> ParamExceptions;

		public FoodPalBadParamsException() : this(string.Empty) { }

		public FoodPalBadParamsException(string badParamsMessage) : this(new ErrorInfoDto() { Type = ErrorInfoType.Error, Message = badParamsMessage }) { }

		public FoodPalBadParamsException(List<string> errors) : this(new ErrorInfoDto() { Type = ErrorInfoType.Error, Message = string.Join("; ", errors) }) { }

		public FoodPalBadParamsException(ErrorInfoDto errorInfoDto, List<Exception> paramExceptions = null) : base(errorInfoDto)
		{
			ParamExceptions = paramExceptions;
		}
	}
}
