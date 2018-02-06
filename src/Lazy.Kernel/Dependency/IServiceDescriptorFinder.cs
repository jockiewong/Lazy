using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependency
{
    /// <summary>
    /// 服务查找器
    /// </summary>
    public interface IServiceDescriptorFinder
    {

        IEnumerable<ServiceDescriptor> FindFromAssembly(Assembly assembly);
    }
}
