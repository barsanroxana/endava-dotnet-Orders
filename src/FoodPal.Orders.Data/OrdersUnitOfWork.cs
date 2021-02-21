using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Data.Repositories;
using System;

namespace FoodPal.Orders.Data
{
	public class OrdersUnitOfWork : IOrdersUnitOfWork
	{
		private readonly Lazy<IOrdersRepository> _ordersRepository;
		private readonly Lazy<IOrderItemsRepository> _orderItemsRepository;
		private readonly Lazy<IDeliveryDetailsRepository> _deliveryDetailsRepository;

		public OrdersUnitOfWork(OrdersContext dbContext)
		{
			_ordersRepository = new Lazy<IOrdersRepository>(new OrdersRepository(dbContext));
			_orderItemsRepository = new Lazy<IOrderItemsRepository>(new OrderItemsRepository(dbContext));
			_deliveryDetailsRepository = new Lazy<IDeliveryDetailsRepository>(new DeliveryDetailsRepository(dbContext));
		}

		public IOrdersRepository OrdersRepository => _ordersRepository.Value;

		public IOrderItemsRepository OrderItemsRepository => _orderItemsRepository.Value;

		public IDeliveryDetailsRepository DeliveryDetailsRepository => _deliveryDetailsRepository.Value;

	}
}
