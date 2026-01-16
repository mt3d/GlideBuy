namespace GlideBuy.Support.Models
{
	public abstract record BaseSearchModel : IPagingRequestModel
	{
		public int Page => (Start / Length) + 1;

		public int PageSize => Length;

		// Skip a number of rows.
		public int Start { get; set; }

		// Sets the page length.
		public int Length { get; set; } = 10;
	}
}
