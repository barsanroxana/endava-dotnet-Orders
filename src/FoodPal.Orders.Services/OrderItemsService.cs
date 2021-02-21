using FoodPal.Orders.Data;
using FoodPal.Orders.Dtos;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Exceptions;
using FoodPal.Orders.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services
{
	public class OrderItemsService : BaseService, IOrderItemsService
	{
		private readonly IOrdersUnitOfWork _orderUoW;

		public OrderItemsService(IOrdersUnitOfWork unitOfWork)
		{
			_orderUoW = unitOfWork;
		}

		public async Task PatchOrderItem(int orderId, int orderItemId, GenericPatchDto orderItemPatch)
		{
			ParameterChecks(new (Func<bool>, Exception)[]
			{
				( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive")),
				( () => orderItemId > 0, new ArgumentOutOfRangeException(nameof(orderItemId), $"{nameof(orderItemId)} must be positive"))
			});

			var orderItem = await _orderUoW.OrderItemsRepository.GetOrderItemAsync(orderId, orderItemId);

			if (orderItem is null) throw new FoodPalNotFoundException($"OrderItemId='{orderItemId}' OrderId='{orderId}'");

			switch (orderItemPatch.PropertyName.ToLowerInvariant())
			{
				case "status":
					await _orderUoW.OrderItemsRepository.UpdateStatusAsync(orderItem, ParseOrderItemStatus(orderItemPatch.PropertyValue.ToString()));
					break;

				default:
					throw new Exception($"Patch is not supported for property name '{orderItemPatch.PropertyName}'");
			}
		}

		#region Private Methods

		private OrderItemStatus ParseOrderItemStatus(string orderItemStatus)
		{
			if (Enum.TryParse<OrderItemStatus>(orderItemStatus, true, out var newOrderItemStatus))
				return newOrderItemStatus;

			throw new Exception($"Cannot parse order item status '{orderItemStatus}'");
		}

		#endregion
	}
}
