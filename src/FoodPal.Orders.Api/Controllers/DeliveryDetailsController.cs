using FoodPal.Orders.Dtos;
using FoodPal.Orders.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodPal.Orders.Api.Controllers
{
	/// <summary>
	/// Providers API methods for handling order item requests.
	/// </summary>
	public class DeliveryDetailsController : ApiBaseController
	{
		private readonly IDeliveryDetailsService _deliveryDetailsService;

		/// <summary>
		/// Constructor for Delivery Details controller.
		/// </summary>
		public DeliveryDetailsController(IDeliveryDetailsService deliveryDetailsService)
		{
			_deliveryDetailsService = deliveryDetailsService;
		}

		[HttpGet]
		[Route("{orderId}")]
		[ProducesResponseType(typeof(DeliveryDetailsDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> GetDeliveryDetailsByOrderId(int orderId)
		{
			var orderResult = await _deliveryDetailsService.GetDeliveryDetailsByOrderIdAsync(orderId);
			return Ok(orderResult);
		}
	}
}
