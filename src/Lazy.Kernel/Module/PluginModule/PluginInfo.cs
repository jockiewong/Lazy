using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    public class PluginInfo
    {
        /// <summary>
        /// 插件程序集
        /// </summary>
        public Assembly PluginAssembly { get; set; }

        /// <summary>
        /// 插件初始化器
        /// </summary>
        public IPluginInitialize PluginInitialize { get; set; }
    }
}
