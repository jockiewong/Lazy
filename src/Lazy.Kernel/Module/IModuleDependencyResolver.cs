using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块依赖解析器
    /// </summary>
    public interface IModuleDependencyResolver
    {
        /// <summary>
        /// 从入口程序集解析模块依赖关系
        /// </summary>
        /// <param name="entryAssembly"></param>
        /// <returns></returns>
        ModuleDescriptor DependencyResolve(Assembly entryAssembly);
    }
}
