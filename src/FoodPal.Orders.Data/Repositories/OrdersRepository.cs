using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly OrdersContext _ordersContext;

		public OrdersRepository(OrdersContext ordersContext)
		{
			_ordersContext = ordersContext;
		}

		public async Task<Order> CreateAsync(Order newOrder)
		{
			if (newOrder is null) throw new ArgumentNullException(nameof(newOrder));

			try
			{
				await _ordersContext.AddAsync(newOrder);
				await _ordersContext.SaveChangesAsync();

				return newOrder;
			}
			catch (Exception ex)
			{
				throw new Exception($"Order could not be saved. Reason:{ex.Message}", ex);
			}
		}

		public async Task<Order> GetByIdAsync(int orderId)
		{
			return await _ordersContext.Orders
				.Include(x => x.Items)
				.Include(x => x.DeliveryDetails)
				.FirstOrDefaultAsync(x => x.Id == orderId);
		}

		public async Task<(IEnumerable<Order> Orders, int AllOrdersCount)> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize)
		{
			IQueryable<Order> ordersQuery = _ordersContext.Orders.AsNoTracking();

			if (!string.IsNullOrEmpty(customerId))
				ordersQuery = _ordersContext.Orders.Where(x => x.CustomerId == customerId);

			if (status.HasValue)
				ordersQuery = ordersQuery.Where(x => x.Status == status.Value);

			var paginatedOrdersQuery = ordersQuery.Skip(page * pageSize).Take(pageSize);

			var ordersList = await paginatedOrdersQuery.ToListAsync();
			var allOrdersCount = await ordersQuery.CountAsync();

			return (ordersList, allOrdersCount);
		}

		public async Task<OrderStatus?> GetStatusAsync(int orderId)
		{
			var orderStatus = await _ordersContext.Orders.Where(x => x.Id == orderId).Select(x => x.Status).ToListAsync();
			return orderStatus.FirstOrDefault();
		}

		public async Task UpdateStatusAsync(Order orderEntity, OrderStatus newStatus)
		{
			orderEntity.Status = newStatus;
			await _ordersContext.SaveChangesAsync();
		}
	}
}
