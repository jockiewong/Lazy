using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Plugin
{
    /// <summary>
    /// 插件模型
    /// </summary>
    public class PluginModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 插件程序集dll文件名称
        /// </summary>
        public string DllFileName { get; set; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 插件依赖id集合
        /// </summary>
        public List<string> Dependencies { get; set; }

        /// <summary>
        /// 插件类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 插件是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
