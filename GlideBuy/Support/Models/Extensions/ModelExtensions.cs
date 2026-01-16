using GlideBuy.Core;

namespace GlideBuy.Support.Models.Extensions
{
	public static class ModelExtensions
	{
		public static IPagedList<T> ToPagedList<T>(this IList<T> list, IPagingRequestModel pagingRequestModel)
		{
			return new PagedList<T>(list, pagingRequestModel.Page - 1, pagingRequestModel.PageSize);
		}
	}
}
