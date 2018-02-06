using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块依赖关系解析器
    /// </summary>
    public interface IModuleDependencyResolver
    {
        /// <summary>
        /// 从入口程序集解析出所有的依赖信息
        /// </summary>
        /// <param name="entryAssemblies"></param>
        /// <returns></returns>
        ModuleDependencyResolvedWrapper DependencyResolve(IEnumerable<Assembly> entryAssemblies);
    }
}
