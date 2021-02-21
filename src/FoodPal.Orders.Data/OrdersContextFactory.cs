using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FoodPal.Orders.Data
{
	public class OrdersContextFactory : IDesignTimeDbContextFactory<OrdersContext>, IOrdersContextFactory
	{
		public OrdersContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var contextOptionsBuilder = new DbContextOptionsBuilder();
			var connectionString = configuration.GetConnectionString("OrdersConnectionString");

			contextOptionsBuilder.UseSqlServer(connectionString);

			return new OrdersContext(contextOptionsBuilder.Options);
		}

		public OrdersContext CreateDbContext(string connectionString)
		{
			var dbContextOptionsBuilder = new DbContextOptionsBuilder().UseSqlServer(connectionString);

			return new OrdersContext(dbContextOptionsBuilder.Options);
		}
	}
}
