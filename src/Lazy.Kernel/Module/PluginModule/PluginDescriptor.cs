using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    public class PluginDescriptor
    {
        /// <summary>
        /// 插件程序集
        /// </summary>
        public Assembly PluginAssembly { get; set; }

        /// <summary>
        /// 插件依赖解析器
        /// </summary>
        public IPluginDependAssemblyResolver DependAssemblyResolver { get; set; }
    }
}
