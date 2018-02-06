using Lazy.Kernel.Dependency;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel
{
    /// <summary>
    /// 框架启动参数
    /// </summary>
    public class StartupOptions
    {

        /// <summary>
        /// 服务查找器清单,默认为ConventionServiceDescriptorFinder
        /// </summary>
        public ICollection<IServiceDescriptorFinder> ServiceFinder { get; } = new List<IServiceDescriptorFinder>() {
            new ConventionServiceDescriptorFinder()
        };


        /// <summary>
        /// 插件程序集清单
        /// </summary>
        public ICollection<Assembly> PluginAssemblies { get; } = new List<Assembly>();
    }
}
