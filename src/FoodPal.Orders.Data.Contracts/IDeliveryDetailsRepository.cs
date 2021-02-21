using FoodPal.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Contracts
{
    public interface IDeliveryDetailsRepository
    {
        Task<DeliveryDetails> GetByOrderIdAsync(int orderId);
    }
}
