using FoodPal.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
    public interface IDeliveryDetailsService
    {
        Task<DeliveryDetailsDto> GetDeliveryDetailsByOrderIdAsync(int orderId);
    }
}
