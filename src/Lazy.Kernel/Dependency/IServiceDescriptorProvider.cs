using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependency
{
    /// <summary>
    /// 服务提供器
    /// </summary>
    public interface IServiceDescriptorProvider
    {
        /// <summary>
        /// 从程序集中查找服务
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="serviceContainSelf">是否包含实现类本身</param>
        /// <returns></returns>
        IEnumerable<ServiceDescriptor> FromAssembly(Assembly assembly, bool serviceContainSelf);
    }
}
