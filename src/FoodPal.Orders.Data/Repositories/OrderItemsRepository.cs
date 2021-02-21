using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
	public class OrderItemsRepository : IOrderItemsRepository
	{
		private readonly OrdersContext _ordersContext;

		public OrderItemsRepository(OrdersContext ordersContext)
		{
			_ordersContext = ordersContext;
		}

		public async Task<OrderItem> GetOrderItemAsync(int orderId, int orderItemId)
		{
			var orderItem = await _ordersContext.OrderItems.Where(x => x.OrderId == orderId && x.Id == orderItemId).FirstOrDefaultAsync();
			return orderItem;
		}

		public async Task<List<OrderItem>> GetItemsAsync(int orderId)
		{
			var orderItems = await _ordersContext.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
			return orderItems;
		}

		public async Task UpdateStatusAsync(OrderItem orderItemEntity, OrderItemStatus newStatus)
		{
			orderItemEntity.Status = newStatus;
			await _ordersContext.SaveChangesAsync();
		}
	}
}
