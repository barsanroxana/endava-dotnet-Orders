using FoodPal.Orders.Dtos;
using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Mappers
{
	internal class OrderStatusProfile : AbstractProfile
	{
		public OrderStatusProfile()
		{
			CreateMap<OrderStatus, StatusDto>()
				.ForMember(d => d.StatusId, op => op.MapFrom(o => (int)o))
				.ForMember(d => d.StatusName, op => op.MapFrom(o => o.ToString()));

			CreateMap<OrderItemStatus, StatusDto>()
				.ForMember(d => d.StatusId, op => op.MapFrom(o => (int)o))
				.ForMember(d => d.StatusName, op => op.MapFrom(o => o.ToString()));
		}
	}
}
