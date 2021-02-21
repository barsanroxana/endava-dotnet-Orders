using FoodPal.Orders.Data.Contracts;

namespace FoodPal.Orders.Data
{
	public interface IOrdersUnitOfWork
	{
		IOrdersRepository OrdersRepository { get; }

		IOrderItemsRepository OrderItemsRepository { get; }

		IDeliveryDetailsRepository DeliveryDetailsRepository { get; }
	}
}
