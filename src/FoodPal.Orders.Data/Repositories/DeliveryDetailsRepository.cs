using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
    class DeliveryDetailsRepository : IDeliveryDetailsRepository
    {
        private readonly OrdersContext _ordersContext;

        public DeliveryDetailsRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<DeliveryDetails> GetByOrderIdAsync(int orderId)
        {
            var orderDeliveryDetails = await _ordersContext.Orders
                .Include(o => o.DeliveryDetails)
                .SingleOrDefaultAsync(o => o.Id == orderId);

            return orderDeliveryDetails.DeliveryDetails;
        }
    }
}
