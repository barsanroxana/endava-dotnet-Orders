using FoodPal.Orders.Data.Configurations;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodPal.Orders.Data
{
	public class OrdersContext : DbContext
	{
		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

		public OrdersContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);

			// Alternative configuration:
			//modelBuilder.ApplyConfiguration<Order>(new OrderEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration<Order>(new OrderItemEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration<Order>(new DeliveryDetailsEntityTypeConfiguration());
		}
	}
}
