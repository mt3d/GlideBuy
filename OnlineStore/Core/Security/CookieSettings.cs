using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Security
{
	public class CookieSettings : ISettings
	{
		public int CustomerCookieExpiresHours { get; set; } = 1;
	}
}
