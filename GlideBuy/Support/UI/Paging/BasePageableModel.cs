using GlideBuy.Core;

namespace GlideBuy.Support.UI.Paging
{
	public abstract record BasePageableModel : IPageableModel
	{
		public virtual void LoadPagedList<T>(IPagedList<T> pagedList)
		{
			FirstItemIndex = (pagedList.PageIndex * pagedList.PageSize) + 1;
			// First case: The total count is smaller, so the last few items don't complete a full page.
			// Second case: the current index + page size don't exceed the total count.
			LastItemIndex = Math.Min(pagedList.TotalCount, (pagedList.PageIndex * pagedList.PageSize) + pagedList.PageSize);

			HasNextPage = pagedList.HasNextPage;
			HasPreviousPage = pagedList.HasPreviousPage;

			PageNumber = pagedList.PageIndex;
			PageSize = pagedList.PageSize;

			TotalItems = pagedList.TotalCount;
			TotalPages = pagedList.TotalPages;
		}

		public int PageIndex
		{
			get
			{
				if (PageNumber > 0)
					return PageNumber - 1;

				return 0;
			}
		}

		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public int TotalItems { get; set; }

		public int TotalPages { get; set; }

		public int FirstItemIndex { get; set; }

		public int LastItemIndex { get; set; }

		public bool HasPreviousPage { get; set; }

		public bool HasNextPage { get; set; }
	}
}
