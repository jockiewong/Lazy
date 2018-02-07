using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    /// <summary>
    /// 插件程序,程序集解析失败时,加载插件的依赖程序集解析器
    /// </summary>
    public interface IPluginDependAssemblyResolver
    {
        /// <summary>
        /// 解析插件依赖程序集
        /// </summary>
        /// <param name="pluginAssembly">插件程序集</param>
        /// <param name="sender"><see cref="ResolveEventHandler"/>事件的sender</param>
        /// <param name="resolveEventArgs"><see cref="ResolveEventHandler"/>事件的args</param>
        /// <returns>依赖程序集</returns>
        Assembly AssemblyResolve(Assembly pluginAssembly, object sender, ResolveEventArgs resolveEventArgs);
    }
}
