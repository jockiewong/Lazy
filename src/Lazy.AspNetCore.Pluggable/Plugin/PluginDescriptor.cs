using Lazy.Kernel.Module.PluginModule;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Plugin
{
    /// <summary>
    /// 插件自描述
    /// </summary>
    public class PluginDescriptor : PluginInfo
    {
        /// <summary>
        /// 插件描述文件位置
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// 插件id,使用插件的文件夹名称
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 插件模型数据
        /// </summary>
        public PluginModel PluginModel { get; set; }
    }
}
