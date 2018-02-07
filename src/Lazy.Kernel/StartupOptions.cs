using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module.PluginModule;
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
        /// 服务提供器集合,默认为ConventionServiceDescriptorFinder
        /// </summary>
        public ICollection<IServiceDescriptorProvider> ServiceProvider { get; } = new List<IServiceDescriptorProvider>() {
            new ConventionServiceDescriptorProvider()
        };

        /// <summary>
        /// 插件集合
        /// </summary>
        public PluginCollection Plugins { get; } = new PluginCollection();

        /// <summary>
        /// 服务注册时是否包含实现类本身,默认为false
        /// <para>如:IUser的实现类User,是否注册User类为服务,默认不注册</para>
        /// </summary>
        public bool ServiceContainSelf { get; set; } = false;
    }
}
