using AutoMapper;
using FoodPal.Orders.Data;
using FoodPal.Orders.Dtos;
using FoodPal.Orders.Exceptions;
using FoodPal.Orders.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services
{
    public class DeliveryDetailsService : BaseService, IDeliveryDetailsService
    {
        private readonly IOrdersUnitOfWork _orderUoW;
        private readonly IMapper _mapper;

        public DeliveryDetailsService(IOrdersUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderUoW = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DeliveryDetailsDto> GetDeliveryDetailsByOrderIdAsync(int orderId)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive")),
            });

            var orderDeliveryDetails = await _orderUoW.DeliveryDetailsRepository.GetByOrderIdAsync(orderId);

            if (orderDeliveryDetails is null) throw new FoodPalNotFoundException(orderId.ToString());

            return _mapper.Map<DeliveryDetailsDto>(orderDeliveryDetails);
        }
    }
}
