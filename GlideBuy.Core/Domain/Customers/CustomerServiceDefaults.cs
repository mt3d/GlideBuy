using GlideBuy.Core.Caching;

namespace GlideBuy.Core.Domain.Customers
{
	public class CustomerServiceDefaults
	{
		public static CacheKey CustomerRolesBySystemNameCacheKey => new("GlideBuy.customerrole.bysystemname.{0}");
	}
}
