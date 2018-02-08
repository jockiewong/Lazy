using Lazy.Kernel.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Lazy.Kernel.Dependency;

namespace Lazy.Kernel
{
    /// <summary>
    /// 核心服务安装器
    /// </summary>
    class KernelServiceInstaller
    {
        public static void Installer(IServiceCollection services)
        {
            services.TryAddSingleton<IIocManager>(r => new IocManager(services));
            services.TryAddSingleton<IModuleDependencyResolver, ModuleDependencyResolver>();
            services.TryAddSingleton<IModuleManager, ModuleManagaer>();
            services.TryAddTransient(typeof(IModuleOptionProvider<>), typeof(DefaultModuleOptionProvider<>));
        }
    }
}
