using FoodPal.Orders.Dtos;
using FoodPal.Orders.Models;

namespace FoodPal.Orders.Mappers
{
	internal class OrderProfile : AbstractProfile
	{
		public OrderProfile()
		{
			CreateMap<NewOrderDto, Order>();
			CreateMap<Order, OrderDto>().ForMember(x => x.OrderStatus, o => o.MapFrom(d => d.Status));
		}
	}
}
