using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependcy
{
    public class ConventionServiceDescriptorFinder : IConventionServiceDescriptorFinder
    {
        public IEnumerable<ServiceDescriptor> AddByConvention(Assembly assembly)
        {
            var types = assembly.GetTypesSafely()
              .Where(r => !r.IsAbstract && r.IsClass && r.BaseType != null)
              .Select(r => r.GetTypeInfo())
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
                    if (IsConvetionInterfaceType(interfaceType))
                        continue;

                    if (type.IsGenericTypeDefinition)
                    {
                        var arg2 = interfaceType.GetGenericArguments();

                        if (GenericArgumentsIsMatch(arg1, arg2))
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

        /// <summary>
        /// 泛型参数是否匹配
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private bool GenericArgumentsIsMatch(Type[] arg1, Type[] arg2)
        {
            if (arg1 == null || arg1.Length == 0)
                return false;

            if (arg2 == null || arg2.Length == 0)
                return false;

            if (arg1.Length != arg2.Length)
                return false;

            for (int i = 0; i < arg1.Length; i++)
            {
                if (arg1[i] != arg2[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否为约定的接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsConvetionInterfaceType(Type type)
        {
            if (type == typeof(ITransient) ||
                type == typeof(ISingleton) ||
                type == typeof(IScoped))
                return true;
            return false;
        }
    }
}
