namespace GlideBuy.Support.Mvc.Routing
{
	public interface ICustomUrlHelper
	{
		Task<string?> RouteGenericUrlAsync<T>(object? values = null);
	}
}
