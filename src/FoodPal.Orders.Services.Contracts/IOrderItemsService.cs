using FoodPal.Orders.Dtos;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
	public interface IOrderItemsService
	{
		Task PatchOrderItem(int orderId, int orderItemId, GenericPatchDto orderItemPatch);
	}
}
