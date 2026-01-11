namespace GlideBuy.Services.Plugins
{
	// Could be used to describe any extension (a plugin or a theme)
	public interface IDescriptor
	{
		string SystemName { get; set; }

		string FriendlyName { get; set; }
	}
}
