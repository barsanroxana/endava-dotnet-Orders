using FoodPal.Orders.Dtos;
using FoodPal.Orders.Models;

namespace FoodPal.Orders.Mappers
{
	internal class DeliveryDetailsProfile : AbstractProfile
	{
		public DeliveryDetailsProfile()
		{
			CreateMap<DeliveryDetailsDto, DeliveryDetails>().ReverseMap();
		}
	}
}
