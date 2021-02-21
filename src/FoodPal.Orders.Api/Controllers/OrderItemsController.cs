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
	public class OrderItemsController : ApiBaseController
	{
		private readonly IOrderItemsService _orderItemService;

		/// <summary>
		/// Constructor for Orders controller.
		/// </summary>
		public OrderItemsController(IOrderItemsService orderItemService)
		{
			_orderItemService = orderItemService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="orderItemId"></param>
		/// <param name="orderPatchDto"></param>
		/// <returns></returns>
		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> PatchOrderItem(int orderId, int orderItemId, [FromQuery] GenericPatchDto orderPatchDto)
		{
			await _orderItemService.PatchOrderItem(orderId, orderItemId, orderPatchDto);
			return Ok();
		}
	}
}
