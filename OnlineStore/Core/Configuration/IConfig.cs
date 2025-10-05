namespace OnlineStore.Core.Configuration
{
	public interface IConfig
	{
		/**
		 * Each configuration has a section name.
		 * 
		 * The name is the same as the type name (say, HostingConfig).
		 */
		string Name => GetType().Name;

		public int GetOrder() => 1;
	}
}
