using System.Net;

namespace GlideBuy.Support.Extensions
{
	public static class HttpRequestExtensions
	{
		public static bool IsPostRequest(this HttpRequest request)
		{
			return request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
