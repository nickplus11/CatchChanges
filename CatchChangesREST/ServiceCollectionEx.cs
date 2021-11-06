using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CatchChangesREST
{
    public static class ServiceCollectionEx
    {
        public static void RegisterAll<TInterface>(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var clientImplementations = assemblies.SelectMany(a =>
                a.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(TInterface))));
            foreach (var clientImplementation in clientImplementations)
            {
                services.Add(new ServiceDescriptor(typeof(TInterface), clientImplementation, serviceLifetime));
            }
        }
    }
}