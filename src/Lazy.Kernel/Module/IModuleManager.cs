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
        /// 所有的模块,平行结构
        /// </summary>
        ICollection<ModuleDescriptor> AllModule { get; }

        /// <summary>
        /// 从入口程序集与启动参数初始化模块
        /// </summary>
        /// <param name="entryAssembly"></param>
        /// <param name="options"></param>
        void Init(Assembly entryAssembly, StartupOptions options);
    }
}
