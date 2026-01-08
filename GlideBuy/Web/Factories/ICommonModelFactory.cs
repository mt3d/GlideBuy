using GlideBuy.Models.Common;
using GlideBuy.Web.Models.Common;

namespace GlideBuy.Web.Factories
{
	public interface ICommonModelFactory
	{
		Task<LogoModel> PrepareLogoModelAsync();

		Task<ShoppingCartButtonModel> PrepareCartButtonModelAsync();
	}
}
