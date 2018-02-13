using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.AspNetCore.Pluggable.Plugin
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        /// 所有已加载的插件集合
        /// </summary>
        PluginCollection Plugins { get; }

        /// <summary>
        /// 从插件源中加载所有的插件
        /// </summary>
        /// <param name="pluginSourceLocation">插件源地址,相对路径</param>
        void LoadAllPlugin(string pluginSourceLocation);

        /// <summary>
        /// 根据id获取插件
        /// </summary>
        /// <param name="id">插件id</param>
        /// <returns></returns>
        PluginDescriptor GetPlugin(string id);

        /// <summary>
        /// 根据程序集获取插件
        /// </summary>
        /// <param name="pluginAssembly">插件程序集</param>
        /// <returns></returns>
        PluginDescriptor GetPlugin(Assembly pluginAssembly);

        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="id">插件id</param>
        /// <returns></returns>
        Task EnablePluginAsync(string id);

        /// <summary>
        /// 禁用插件
        /// </summary>
        /// <param name="id">插件id</param>
        /// <returns></returns>
        Task DisablePluginAsync(string id);
    }
}
