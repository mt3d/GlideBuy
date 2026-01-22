using GlideBuy.Core.Domain.Customers;
using Microsoft.AspNetCore.Http;

namespace GlideBuy.Services.Payments
{
	public class PaymentPluginManager : IPaymentPluginManager
	{
		public PaymentPluginManager()
		{
		}

		public Task<IList<int>> GetRestrictedCountryIdsAsync(IPaymentMethod paymentMethod)
		{
			throw new NotImplementedException();
		}

		public bool IsPluginActive(IPaymentMethod paymentMethod)
		{
			throw new NotImplementedException();
		}

		public bool IsPluginActive(IPaymentMethod plugin, List<string> systemNames)
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsPluginActiveAsync(string systemName, Customer? customer = null)
		{
			throw new NotImplementedException();
		}

		public async Task<IList<IPaymentMethod>> LoadActivePluginsAsync(Customer? customer = null, int countryId = 0)
		{
			IList<IPaymentMethod> paymentMethods = new List<IPaymentMethod>();


			return paymentMethods;
		}

		public Task<IList<IPaymentMethod>> LoadActivePluginsAsync(List<string> systemNames)
		{
			throw new NotImplementedException();
		}

		public Task<IList<IPaymentMethod>> LoadAllPluginsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IPaymentMethod> LoadPluginBySystemNameAsync(string systemName)
		{
			throw new NotImplementedException();
		}

		public Task SaveRestrictedCountryIdsAsync(IPaymentMethod paymentMethod, IList<int> countryIds)
		{
			throw new NotImplementedException();
		}
	}
}
