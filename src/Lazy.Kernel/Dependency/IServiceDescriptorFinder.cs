using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependency
{
    public interface IServiceDescriptorFinder
    {

        IEnumerable<ServiceDescriptor> FindFromAssembly(Assembly assembly);
    }
}
