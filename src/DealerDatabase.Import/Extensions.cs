using System.Reflection;

using DealerDatabase.Import.Data;

using Microsoft.Extensions.DependencyInjection;

namespace DealerDatabase.Import;

internal static class Extensions
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDealerImportersFromAssembly(Assembly assembly)
		{
			var serviceType = typeof(IDealerImporter);
			var types = assembly.GetTypes().Where(type => !type.IsAbstract && serviceType.IsAssignableFrom(type));

			foreach (var type in types)
				services.AddSingleton(serviceType, type);

			return services;
		}
	}
}