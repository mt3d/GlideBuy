using GlideBuy.Support.Models;

namespace GlideBuy.Areas.Admin.Models.Plugins
{
	public record PluginSearchModel : BaseSearchModel
	{
		public bool NeedRestart { get; set; }
	}
}
