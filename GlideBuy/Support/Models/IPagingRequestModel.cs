namespace GlideBuy.Support.Models
{
	public interface IPagingRequestModel
	{
		public int Page { get; }

		public int PageSize { get; }
	}
}
