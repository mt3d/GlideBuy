namespace GlideBuy.Web.UI.Paging
{
	public interface IPageableModel
	{
		/// <summary>
		/// The index starts from 0.
		/// </summary>
		int PageIndex { get; }

		int PageNumber { get; }

		int PageSize { get; }

		int TotalItems { get; }

		int TotalPages { get; }

		// The index of the first item in the CURRENT PAGE.
		int FirstItemIndex { get; }

		// The index of the last item in the CURRENT PAGE.
		int LastItemIndex { get; }

		bool HasPreviousPage { get; }

		bool HasNextPage { get; }
	}
}
