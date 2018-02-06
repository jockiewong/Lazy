using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块依赖解析器
    /// </summary>
    public class ModuleDependencyResolver : IModuleDependencyResolver
    {
        public ModuleDescriptor DependencyResolve(Assembly entryAssembly)
        {
            throw new NotImplementedException();
        }
    }
}
