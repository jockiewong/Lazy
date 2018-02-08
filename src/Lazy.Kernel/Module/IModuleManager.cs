using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    public interface IModuleManager
    {
        /// <summary>
        /// 已加载的所有模块
        /// </summary>
        ModuleResolvedResult ModuleResolvedResult { get; }

        /// <summary>
        /// 加载所有的模块
        /// </summary>
        /// <param name="entryAssembly"></param>
        void LoadAllModule(Assembly entryAssembly);

        /// <summary>
        /// 配置所有的模块服务
        /// </summary>
        /// <param name="lazyBuilder"></param>
        void ConfigureAllModuleService(ILazyBuilder lazyBuilder);

        /// <summary>
        /// 初始化所有的模块
        /// </summary>
        /// <param name="serviceProvider"></param>
        void InitAllModule(IServiceProvider serviceProvider);
    }
}
