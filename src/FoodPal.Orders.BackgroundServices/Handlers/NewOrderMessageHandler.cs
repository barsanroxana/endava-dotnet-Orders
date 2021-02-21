using AutoMapper;
using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.BackgroundServices.Settings;
using FoodPal.Orders.Data;
using FoodPal.Orders.Dtos;
using FoodPal.Orders.Dtos.External;
using FoodPal.Orders.Enums;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundServices.Handlers
{
	public class NewOrderMessageHandler : BaseMessageHandler, IMessageHandler
	{
		private readonly ILogger<NewOrderMessageHandler> _logger;
		private readonly HttpProviderEndpoints _httpProviderEndpoints;
		private readonly IMapper _mapper;
		private readonly IOrdersUnitOfWork _orderUoW;
		private readonly IMessageBroker _messageBroker;

		public NewOrderMessageHandler(ILogger<NewOrderMessageHandler> logger, IOptions<HttpProviderEndpoints> httpProviderEndpoints, IMapper mapper, IOrdersUnitOfWork unitOfWork, IMessageBroker messageBroker)
		{
			_logger = logger;
			_httpProviderEndpoints = httpProviderEndpoints.Value;
			_mapper = mapper;
			_orderUoW = unitOfWork;
			_messageBroker = messageBroker;
		}

		public async Task ExecuteAsync<TPayload>(MessageBrokerEnvelope<TPayload> messageEnvelope)
		{
			var payload = GetEnvelopePayload<TPayload, NewOrderDto>(messageEnvelope);
			var persistedOrder = await SaveNewOrderAsync(payload);

			var grouppedOrderItems = persistedOrder.Items.GroupBy(x => x.ProviderId).ToDictionary(k => k.Key, v => v.ToList());

			foreach (var providerItems in grouppedOrderItems)
			{
				switch (providerItems.Key.ToLower())
				{
					case "xyz":
					case "prestopizza":
						await SendOrderRequestToProviderViaMessageBrokerAsync(providerItems.Key.ToLower(), persistedOrder.Id, providerItems.Value);
						await UpdateOrderItemsStatus(providerItems.Value);
						break;

					case "kfc":
						await SendOrderRequestToProviderViaMessageBrokerAsync(providerItems.Key.ToLower(), persistedOrder.Id, providerItems.Value);
						await UpdateOrderItemsStatus(providerItems.Value);
						break;

					case "chefsexperience":
						await SendOrderRequestToProviderViaHttpAsync(_httpProviderEndpoints.ChefsExperienceBaseEndpoint, persistedOrder.Id, providerItems.Value);
						await UpdateOrderItemsStatus(providerItems.Value);
						break;

					case "greekcruisine":
						await SendOrderRequestToProviderViaHttpAsync(_httpProviderEndpoints.GreekCruisineBaseEndpoint, persistedOrder.Id, providerItems.Value);
						await UpdateOrderItemsStatus(providerItems.Value);
						break;

					default:
						throw new Exception($"Handling order items for provider '{providerItems.Key}' is not supported.");
				}
			}

			await UpdateOrderStatus(persistedOrder);
		}

		#region Private Methods

		private async Task<Order> SaveNewOrderAsync(NewOrderDto newOrderDto)
		{
			var newOrderModel = _mapper.Map<Order>(newOrderDto);

			newOrderModel.CreatedAt = newOrderModel.LastUpdatedAt = DateTime.Now;
			newOrderModel.Status = OrderStatus.New;

			foreach (var orderItem in newOrderModel.Items)
			{
				orderItem.Status = OrderItemStatus.New;
			}

			return await _orderUoW.OrdersRepository.CreateAsync(newOrderModel);
		}

		private async Task UpdateOrderItemsStatus(List<OrderItem> orderItems)
		{
			foreach (var orderItem in orderItems)
			{
				await _orderUoW.OrderItemsRepository.UpdateStatusAsync(orderItem, OrderItemStatus.InProgress);
			}
		}

		private async Task UpdateOrderStatus(Order order)
		{
			await _orderUoW.OrdersRepository.UpdateStatusAsync(order, OrderStatus.InProgress);
		}

		private async Task SendOrderRequestToProviderViaMessageBrokerAsync(string providerId, int orderId, List<OrderItem> orderItems)
		{
			var messageContent = new MessageBrokerExternalOrderRequestDto
			{
				OrderId = orderId,
				OrderItems = orderItems.Select(x => new MessageBrokerExternalOrderItemRequestDto { OrderItemId = x.Id, Name = x.Name, Quantity = x.Quantity }).ToList()
			};
			var payload = new MessageBrokerEnvelope<MessageBrokerExternalOrderRequestDto>(MessageTypes.ProviderNewOrder, messageContent);

			await _messageBroker.SendMessageAsync($"provider-{providerId}-request", payload);
		}

		private async Task SendOrderRequestToProviderViaHttpAsync(string providerUri, int orderId, List<OrderItem> orderItems)
		{
			var payload = new HttpExternalOrderRequestDto
			{
				OrderId = orderId,
				OrderItems = orderItems.Select(x => new HttpExternalOrderItemRequestDto
				{
					OrderItemId = x.Id,
					Name = x.Name,
					Quantity = x.Quantity,
					CallbackEndpoint = new Uri(
						new Uri(_httpProviderEndpoints.SelfCallbackBaseEndpoint),
						$"OrderItems?orderId={orderId}&orderItemId={x.Id}&PropertyName=status&PropertyValue={(int)OrderItemStatus.Ready}").ToString()
				})
				.ToList()
			};

			var response = await new HttpProxy().PostAsync<HttpExternalOrderRequestDto, string>(providerUri, payload);
		}

		#endregion	
	}
}
