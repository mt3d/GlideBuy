using GlideBuy.Core.Domain.Customers;
using GlideBuy.Services.Plugins;

namespace GlideBuy.Services.Payments
{
	/**
	 * IPluginManager<TPlugin> is a low-level, generic abstraction concerned with
	 * plugin discovery, loading, and basic activation checks. It exposes primitives
	 * like “given this list of system names, tell me which plugins are active”.
	 * IPaymentPluginManager, on the other hand, is a domain-specific specialization.
	 * It raises the level of abstraction by embedding payment-specific rules directly
	 * into the contract, such as country restrictions and store filtering. Instead
	 * of removing or replacing the generic methods, it adds higher-level overloads
	 * that are more convenient and safer for payment-related code.
	 */
	public interface IPaymentPluginManager : IPluginManager<IPaymentMethod>
	{
		Task<IList<IPaymentMethod>> LoadActivePluginsAsync(Customer? customer = null, int countryId = 0);

		bool IsPluginActive(IPaymentMethod paymentMethod);

		Task<bool> IsPluginActiveAsync(string systemName, Customer? customer = null);

		Task<IList<int>> GetRestrictedCountryIdsAsync(IPaymentMethod paymentMethod);

		Task SaveRestrictedCountryIdsAsync(IPaymentMethod paymentMethod, IList<int> countryIds);
	}
}
