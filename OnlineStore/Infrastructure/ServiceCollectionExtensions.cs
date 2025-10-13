using GlideBuy.Infrastructure.StartupConfigurations;
using System.Reflection;

namespace OnlineStore.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static void ConfigureWebApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			// TODO: Configure plugins here in the future.

			// TODO: Add a rate limiter

			// TODO: Configure all services
			TypeFinder finder = new TypeFinder();
			var startupConfigurationClasses = finder.FindClassesByType<IStartupConfiguration>();

			var instances = startupConfigurationClasses
								.Select(startup => (IStartupConfiguration)Activator.CreateInstance(startup))
								.Where(startup => startup != null)
								.OrderBy(startup => (int)(startup.Order));

			foreach (var instance in instances)
			{
				instance.ConfigureServices(services, configuration);
			}

			// TODO: Run startup tasks here.
		}
	}

	public class TypeFinder
	{
		public IEnumerable<Type> FindClassesByType<T>(bool concrete = true)
		{
			var result = new List<Type>();

			try
			{
				IList<Assembly> assemblies = GetAssemblies();

				foreach (var a in assemblies)
				{
					Type[] types = null;
					try
					{
						types = a.GetTypes();
					}
					catch
					{
						// Ignore the exception if we could not load the types of this assembly.
					}

					if (types == null)
						continue;

					foreach(var type in types)
					{
						// TODO: Check for generic type definition and for "open generic"
						if (!typeof(T).IsAssignableFrom(type))
							continue;

						if (type.IsInterface)
							continue;

						if (concrete)
						{
							if (type.IsClass && !type.IsAbstract)
								result.Add(type);
						}
						else
						{
							result.Add(type);
						}
					}
				}
			}
			catch
			{
				// TODO: Handle ReflectionTypeLoadException
				// While GetTypes() return all types, one might not be able to activate them
				// and this could potentionally throw a ReflectionTypeLoadException.
				// See: https://stackoverflow.com/a/29379834/6093615

				// Log a debug message and rethrow?
			}

			return result;
		}

		public IList<Assembly> GetAssemblies()
		{
			// TODO: Check first that all data is loaded.
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}
}
