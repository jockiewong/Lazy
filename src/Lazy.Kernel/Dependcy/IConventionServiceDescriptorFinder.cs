using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Dependcy
{
    public interface IConventionServiceDescriptorFinder
    {

        IEnumerable<ServiceDescriptor>  AddByConvention(Assembly assembly);
    }
}
