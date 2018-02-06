using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel.Dependency
{
    public class BaseOnServiceDescriptorFinder : IServiceDescriptorFinder
    {
        TypeInfo _baseType;
        ServiceLifetime _serviceLifetime;

        public BaseOnServiceDescriptorFinder(Type baseType, ServiceLifetime serviceLifetime)
        {
            _baseType = baseType.GetTypeInfo() ?? throw new ArgumentNullException(nameof(baseType));
            _serviceLifetime = serviceLifetime;
        }

        public IEnumerable<ServiceDescriptor> FindFromAssembly(Assembly assembly)
        {
            var types = assembly
                            .DefinedTypes
                            .Where(r => !r.IsAbstract && r.IsClass && r.BaseType != null && !r.ImplementedInterfaces.IsNullOrEmpty());

            List<ServiceDescriptor> result = new List<ServiceDescriptor>();

            types.ForEach_(type =>
            {
                List<Type> services = new List<Type>();
                services.Add(type);


                if (!_baseType.IsGenericTypeDefinition && type.IsChildTypeOf(_baseType))
                {
                    _baseType.ImplementedInterfaces.ForEach_(interfaceType =>
                    {
                        if (Utils.IsConvetionInterfaceType(interfaceType) || interfaceType == _baseType)
                            return;

                        services.Add(interfaceType);
                    });
                }
                else if (_baseType.IsGenericTypeDefinition && type.IsChildTypeOfGenericType(_baseType))
                {
                    foreach (var interfaceType in type.ImplementedInterfaces)
                    {
                        if (Utils.IsConvetionInterfaceType(interfaceType))
                            continue;

                        var arg1 = type.GetGenericArguments();
                        var arg2 = interfaceType.GetGenericArguments();

                        var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == null)
                            continue;
                        if (Utils.GenericArgumentsIsMatch(arg1, arg2))
                            services.Add(genericTypeDefinition);
                    }
                }
                else
                    return;

                services.Add(type);

                services.ForEach(service =>
                {
                    result.Add(ServiceDescriptor.Describe(service, type, _serviceLifetime));
                });
            });

            return result;
        }
    }
}
