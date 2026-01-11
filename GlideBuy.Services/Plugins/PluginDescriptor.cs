using System.Text.Json.Serialization;

namespace GlideBuy.Services.Plugins
{
	// Could be saved to a file.
	public class PluginDescriptor : PluginDescriptorBaseInfo, IDescriptor
	{
		public virtual string Group { get; set; }

		public virtual string FriendlyName { get; set; }

		public virtual string Author { get; set; }

		public virtual string Description { get; set; }

		[JsonIgnore]
		public virtual bool Installed { get; set; }
	}
}
