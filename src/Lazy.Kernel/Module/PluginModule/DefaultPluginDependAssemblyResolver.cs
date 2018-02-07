using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    /// <summary>
    /// 默认的插件程序集依赖解析器,从插件程序集所在目录中加载依赖
    /// </summary>
    public class DefaultPluginDependAssemblyResolver : IPluginDependAssemblyResolver
    {
        public Assembly AssemblyResolve(Assembly pluginAssembly, object sender, ResolveEventArgs resolveEventArgs)
        {
            var path = Path.GetDirectoryName(pluginAssembly.Location);
            var dll = Path.Combine(path, resolveEventArgs.Name.Split(',')[0] + ".dll");
            if (File.Exists(dll))
                return Assembly.LoadFrom(dll);
            return null;
        }
    }
}
