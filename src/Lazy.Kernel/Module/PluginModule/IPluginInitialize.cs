using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    /// <summary>
    /// 插件初始化
    /// </summary>
    public interface IPluginInitialize
    {
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="pluginAssembly"></param>
        void Init(Assembly pluginAssembly);
    }
}
