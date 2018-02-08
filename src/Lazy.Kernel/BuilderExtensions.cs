using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace Lazy.Kernel
{
    public static class BuilderExtensions
    {

        /// <summary>
        /// 增加lazy框架依赖注入,已注入以下接口,如需替换内部服务,请在调用该方法前注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="entryAssembly">入口程序集</param>
        /// <param name="optionsAction">启动选项设置</param>
        /// <returns></returns>
        public static ILazyBuilder AddLazy<TStartupModuleType>(
            this IServiceCollection services,
            Action<StartupOptions> optionsAction = null)
            where TStartupModuleType : LazyModule
        {
            services.Configure(optionsAction ?? (r => { }));
            LazyBuilder.Init(services);
            KernelServiceInstaller.Installer(services);
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IModuleOptionProvider<StartupOptions>>().GetConfiguredOptions();
                var moduleManager = scope.ServiceProvider.GetRequiredService<IModuleManager>();
                moduleManager.LoadAllModule(typeof(TStartupModuleType).Assembly);
                moduleManager.ConfigureAllModuleService(LazyBuilder.Instance);

                //不同的provider,单例也不是单例,所以replace
                services.Replace(ServiceDescriptor.Singleton(moduleManager));
                //不同的provider,因为LazyBuilder的注册使用的new一个实例,所以单例还是单例
                return LazyBuilder.Instance;
            }
        }

        /// <summary>
        /// 使用lazy框架,将启动lazy框架,初始化模块
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void UseLazy(this IServiceProvider serviceProvider)
        {
            var moduleManager = serviceProvider.GetRequiredService<IModuleManager>();
            moduleManager.InitAllModule(serviceProvider);
        }
    }
}
