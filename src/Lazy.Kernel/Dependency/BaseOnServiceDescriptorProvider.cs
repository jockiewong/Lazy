using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel.Dependency
{
    /// <summary>
    /// 默认实现的一个根据基类查找服务的实现,只会注册接口和自身为服务,如果baseType为泛型类型,会注册这个baseType为服务,如果baseType非泛型,则baseType不会注册为服务,
    /// <para>如:baseType为IUser,那么会注册所有实现了IUser的类型,但不会注册IUser这个接口</para>
    /// <para>如:baseType为IUser&lt;&gt;,那么会注册所有实现了IUser的类型,同时也会注册IUser&lt;&gt;这个泛型接口</para>
    /// <para>该提供器并未添加到默认的提供器集合中,如需使用,请在AddLazy的配置代码中自行添加</para>
    /// </summary>
    public class BaseOnServiceDescriptorProvider : IServiceDescriptorProvider
    {
        TypeInfo _baseType;
        ServiceLifetime _serviceLifetime;

        public BaseOnServiceDescriptorProvider(Type baseType, ServiceLifetime serviceLifetime)
        {
            _baseType = baseType.GetTypeInfo() ?? throw new ArgumentNullException(nameof(baseType));
            _serviceLifetime = serviceLifetime;
        }

        public IEnumerable<ServiceDescriptor> FromAssembly(Assembly assembly, bool serviceContainSelf)
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
                if (serviceContainSelf)
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
