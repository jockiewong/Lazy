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
        public PluginModel(
            bool enabled,
            string name,
            string dllFileName,
            string desciption,
            string author,
            string category,
            string version,
            List<string> dependencies
            )
        {
            this.Enabled = enabled;
            this.Name = name;
            this.DllFileName = dllFileName;
            this.Description = desciption;
            this.Author = author;
            this.Category = category;
            this.Version = version;
            this.Dependencies = dependencies;
        }

        /// <summary>
        /// 插件是否启用
        /// </summary>
        public bool Enabled { get; private set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 插件程序集dll文件名称
        /// </summary>
        public string DllFileName { get; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// 插件类别
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// 插件依赖id集合
        /// </summary>
        public List<string> Dependencies { get; }

        /// <summary>
        /// 设置启用/禁用状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            //TODO:  优化
            Enabled = status;
        }
    }
}
