using GlideBuy.Core.Domain.Customers;
using GlideBuy.Plugin.Payments.CreditCard;
using GlideBuy.Services.Payments;

namespace GlideBuy
{
	public class PaymentPluginManagerMock : IPaymentPluginManager
	{
		// TODO: Remove in the future once plugins are implemented
		private readonly IHttpContextAccessor _contextAccessor;

		public PaymentPluginManagerMock(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
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
			// TODO: Sample implementation for now, until the plugin system is fully created.

			IList<IPaymentMethod> paymentMethods = new List<IPaymentMethod>();

			// TODO: Remove project references in the future.
			var type = typeof(ManualProcessingPaymentMethod);
			Exception innerException;
			foreach (var constructor in type.GetConstructors())
			{
				try
				{
					//try to resolve constructor parameters
					var parameters = constructor.GetParameters().Select(parameter =>
					{
						var context = _contextAccessor?.HttpContext;
						var serviceProvider = context?.RequestServices;

						var service = serviceProvider?.GetService(parameter.ParameterType) ?? throw new Exception("Unknown dependency");
						return service;
					});

					//all is ok, so create instance
					paymentMethods.Add((IPaymentMethod)Activator.CreateInstance(type, parameters.ToArray()));
				}
				catch (Exception ex)
				{
					innerException = ex;
				}
			}

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

		public Task<IPaymentMethod> LoadPluginBySystemName(string systemName)
		{
			throw new NotImplementedException();
		}

		public Task SaveRestrictedCountryIdsAsync(IPaymentMethod paymentMethod, IList<int> countryIds)
		{
			throw new NotImplementedException();
		}
	}
}
