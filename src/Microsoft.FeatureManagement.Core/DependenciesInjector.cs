using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Core
{
    public static class DependenciesInjector
    {
        #region Methods

        public static void AddIInjectableDependencies(this IServiceCollection services, Type objectType)
        {
            List<Type> types = objectType.Assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract &&
                            t.GetTypeInfo().ImplementedInterfaces.Any(i => i == typeof(IInjectable)))
                .Select(t => t).OrderBy(p => p.Name).ToList();

            foreach (Type type in types)
            {
                int max = 0;
                Type interfaceType = null;
                foreach (Type it in type.GetInterfaces())
                {
                    int nombreInterfaceImplIService = it.GetInterfaces().Length;
                    if (it.GetInterfaces().Any(i => i == typeof(IInjectable)) && max < nombreInterfaceImplIService)
                    {
                        max = nombreInterfaceImplIService;
                        interfaceType = it;
                    }
                }

                services.AddTransient(interfaceType, type);
            }
        }

        #endregion
    }
}