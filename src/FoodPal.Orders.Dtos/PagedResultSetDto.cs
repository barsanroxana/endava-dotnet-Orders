using System.Collections.Generic;

namespace FoodPal.Orders.Dtos
{
	public class PagedResultSetDto<T> where T : class
	{
		public IList<T> Data { get; set; }
		public PaginationInfoDto PaginationInfo { get; set; }
	}

	public class PaginationInfoDto
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int Total { get; set; }
	}
}
