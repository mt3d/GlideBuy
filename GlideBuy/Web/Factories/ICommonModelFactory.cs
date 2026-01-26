using GlideBuy.Models.Common;

namespace GlideBuy.Web.Factories
{
	public interface ICommonModelFactory
	{
		Task<LogoModel> PrepareLogoModelAsync();

		Task<ShoppingCartButtonModel> PrepareCartButtonModelAsync();
	}
}
