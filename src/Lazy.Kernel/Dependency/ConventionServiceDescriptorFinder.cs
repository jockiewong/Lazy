using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependency
{
    public class ConventionServiceDescriptorFinder : IServiceDescriptorFinder
    {
        public IEnumerable<ServiceDescriptor> FindFromAssembly(Assembly assembly)
        {
            var types = assembly
                            .DefinedTypes
                            .Where(r => !r.IsAbstract && r.IsClass && r.BaseType != null)
                            .Where(r => !r.ImplementedInterfaces.IsNullOrEmpty());

            List<ServiceDescriptor> result = new List<ServiceDescriptor>();
            types.ForEach_(type =>
            {

                ServiceLifetime serviceLifetime;
                if (type.IsChildTypeOf<ITransient>())
                    serviceLifetime = ServiceLifetime.Transient;
                else if (type.IsChildTypeOf<ISingleton>())
                    serviceLifetime = ServiceLifetime.Singleton;
                else if (type.IsChildTypeOf<IScoped>())
                    serviceLifetime = ServiceLifetime.Scoped;
                else
                    return;

                var arg1 = type.GetGenericArguments();

                List<Type> services = new List<Type>();
                foreach (var interfaceType in type.ImplementedInterfaces)
                {
                    if (Utils.IsConvetionInterfaceType(interfaceType))
                        continue;

                    if (type.IsGenericTypeDefinition)
                    {
                        var arg2 = interfaceType.GetGenericArguments();

                        if (Utils.GenericArgumentsIsMatch(arg1, arg2))
                            services.Add(interfaceType.GetGenericTypeDefinition());
                    }
                    else
                        services.Add(interfaceType);
                }
                services.Add(type);

                services.ForEach(service =>
                {
                    result.Add(ServiceDescriptor.Describe(service, type, serviceLifetime));
                });
            });

            return result;
        }

    }
}
