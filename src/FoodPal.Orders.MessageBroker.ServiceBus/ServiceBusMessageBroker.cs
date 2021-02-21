using Azure.Messaging.ServiceBus;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.MessageBroker.ServiceBus
{
	public class ServiceBusMessageBroker : IMessageBroker
	{
		private readonly string _messageBrokerEndpoint;
		private ServiceBusClient _sbMessageReceiverClient;
		private ServiceBusProcessor _sbProcessor;
		private MessageReceivedEventHandler _messageHandler;

		public ServiceBusMessageBroker(IOptions<MessageBrokerConnectionSettings> connectionSettings)
		{
			_messageBrokerEndpoint = connectionSettings.Value.Endpoint;
		}

		public void RegisterMessageReceiver(string queueName, MessageReceivedEventHandler messageHandler)
		{
			_messageHandler = messageHandler;
			_sbMessageReceiverClient = new ServiceBusClient(_messageBrokerEndpoint);
			_sbProcessor = _sbMessageReceiverClient.CreateProcessor(queueName, new ServiceBusProcessorOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

			_sbProcessor.ProcessMessageAsync += MessageHandlerAsync;
			_sbProcessor.ProcessErrorAsync += ErrorHandlerAsync;
		}

		public async Task SendMessageAsync<TMessage>(string queueName, TMessage message)
		{
			await using (ServiceBusClient sbClient = new ServiceBusClient(_messageBrokerEndpoint))
			{
				var sender = sbClient.CreateSender(queueName);
				var serializedMessage = JsonConvert.SerializeObject(message);

				var sbMessage = new ServiceBusMessage(serializedMessage);

				await sender.SendMessageAsync(sbMessage);
			}
		}

		public async Task StartListenerAsync()
		{
			await _sbProcessor.StartProcessingAsync();
		}

		public async Task StopListenerAsync()
		{
			await _sbProcessor.StopProcessingAsync();
			await _sbMessageReceiverClient.DisposeAsync();
		}

		private async Task MessageHandlerAsync(ProcessMessageEventArgs args)
		{
			var messageAsString = args.Message.Body.ToString();

			await _messageHandler(messageAsString);

			// Commeting this because our queues are configured as ServiceBusReceiveMode.ReceiveAndDelete => thrown exception 
			//await args.CompleteMessageAsync(args.Message);
		}

		private async Task ErrorHandlerAsync(ProcessErrorEventArgs args)
		{
			throw new Exception("Error occurred in ServiceBus message handler", args.Exception);
		}
	}
}
