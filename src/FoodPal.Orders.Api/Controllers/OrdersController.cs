using FoodPal.Orders.Dtos;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodPal.Orders.Api.Controllers
{
	/// <summary>
	/// Providers API methods for handling order requests.
	/// </summary>
	public class OrdersController : ApiBaseController
	{
		private readonly IOrdersService _ordersService;

		/// <summary>
		/// Constructor for Orders controller.
		/// </summary>
		public OrdersController(IOrdersService ordersService)
		{
			_ordersService = ordersService;
		}

		/// <summary>
		/// Returns a paged list of orders.
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="status"></param>
		/// <param name="page">Current result page no.</param>
		/// <param name="pageSize">No. of returned records per page.</param>
		/// <returns>A paginated collection of orders, sorted by last modified date - the most recently updated will be first in the result set.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(PagedResultSetDto<OrderDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> GetOrders(string customerId, OrderStatus? status, int page = 1, int pageSize = 20)
		{
			var orderResult = await _ordersService.GetByFiltersAsync(customerId, status, page, pageSize);
			return Ok(orderResult);
		}

		/// <summary>
		/// Returns the specified order, if exists.
		/// </summary>
		/// <param name="orderId">The order identifier.</param>
		/// <returns>An object containing the order details.</returns>
		[HttpGet]
		[Route("{orderId}")]
		[ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> GetOrderById(int orderId)
		{
			var orderResult = await _ordersService.GetByIdAsync(orderId);
			return Ok(orderResult);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{orderId}/status")]
		[ProducesResponseType(typeof(StatusDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<StatusDto>> GetOrderStatusById(int orderId)
		{
			var orderStatus = await _ordersService.GetStatusAsync(orderId);
			return Ok(orderStatus);
		}

		/// <summary>
		/// Creates an order.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> CreateOrder(NewOrderDto newOrder)
		{
			var requestId = await _ordersService.Create(newOrder);
			return Accepted(requestId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="orderPatchDto"></param>
		/// <returns></returns>
		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> PatchOrder(int orderId, GenericPatchDto orderPatchDto)
		{
			await _ordersService.PatchOrder(orderId, orderPatchDto);
			return Ok();
		}
	}
}