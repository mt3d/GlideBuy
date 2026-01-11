using GlideBuy.Core;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Data.Extensions
{
	public static class AsyncIQueryableExtensions
	{
		public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
		{
			if (source is null)
			{
				return new PagedList<T>(new List<T>(), pageIndex, pageSize);
			}

			pageSize = Math.Max(pageSize, 1);

			var count = await source.CountAsync();
			var data = new List<T>();
			data.AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());

			return new PagedList<T>(data, pageIndex, pageSize);
		}
	}
}
