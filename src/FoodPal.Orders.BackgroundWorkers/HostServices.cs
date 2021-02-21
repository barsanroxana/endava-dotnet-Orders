using FoodPal.Orders.BackgroundServices.Handlers;
using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.BackgroundServices.Settings;
using FoodPal.Orders.BackgroundWorkers.Workers;
using FoodPal.Orders.Data;
using FoodPal.Orders.Mappers;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.MessageBroker.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodPal.Orders.BackgroundWorkers
{
	internal static class HostServices
	{
		public static void Configure(HostBuilderContext hostingContext, IServiceCollection services)
		{
			// AutoMapper
			services.AddAutoMapper(typeof(AbstractProfile).Assembly);

			services.Configure<HttpProviderEndpoints>(x => hostingContext.Configuration.Bind("HttpProviderEndpoints", x));

			// Service Bus message broker
			services.Configure<MessageBrokerConnectionSettings>(x => hostingContext.Configuration.Bind("MessageBrokerSettings", x));
			services.AddTransient<IMessageBroker, ServiceBusMessageBroker>();

			services.AddTransient<IMessageHandlerFactory, MessageHandlerFactory>();
			services.AddTransient<NewOrderMessageHandler>();
			services.AddTransient<ProviderProcessedOrderItemsHandler>();

			var dbConnectionString = hostingContext.Configuration.GetConnectionString("OrdersConnectionString");

			services.AddTransient<IOrdersContextFactory, OrdersContextFactory>();
			services.AddTransient<IOrdersUnitOfWork, OrdersUnitOfWork>(sp =>
				new OrdersUnitOfWork(sp.GetService<IOrdersContextFactory>().CreateDbContext(dbConnectionString)));

			services.AddHostedService<NewOrderWorker>();
			services.AddHostedService<KfcProviderOrderItemsWorker>();
			services.AddHostedService<PrestoPizzaProviderOrderItemsWorker>();
		}
	}
}
