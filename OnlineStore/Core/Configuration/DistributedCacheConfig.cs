namespace GlideBuy.Core.Configuration
{
	public class DistributedCacheConfig : IConfig
	{
		public DistributedCacheType DistributedCacheType { get; protected set; } = DistributedCacheType.RedisSynchronizedMemory;

		public bool Enabled { get; protected set; } = false;
	}
}
