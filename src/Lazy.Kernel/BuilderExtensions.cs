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


namespace Lazy.Kernel
{
    public static class BuilderExtensions
    {

        /// <summary>
        /// 增加lazy框架依赖注入,已注入以下接口,如需替换服务,请在调用该方法前注册服务
        /// <para><see cref="IModuleDependencyResolver"/>模块依赖解析器</para>
        /// <para><see cref="IModuleManager"/>模块管理器</para>
        /// <para><see cref="Lazy.Kernel.Dependency.IIocManager"/>依赖注入管理器</para>
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
            //TODO:..加入日志记录器
            services.Configure(optionsAction ?? (r => { }));

            KernelServiceInstaller.Installer(services);
            StartupOptions options = new StartupOptions();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IConfigureOptions<StartupOptions>>().Configure(options);
                var moduleManager = scope.ServiceProvider.GetRequiredService<IModuleManager>();
                moduleManager.Init(typeof(TStartupModuleType).Assembly, options);

                moduleManager.AllModule.ForEach_(r =>
                {
                    services.TryAddSingleton(r.ModuleType);
                    options.ServiceFinder.ForEach_(a =>
                    {
                        var serviceDescriptors = a.FromAssembly(r.Assembly, options.ServiceContainSelf);
                        services.Add(serviceDescriptors);
                    });
                });

                return scope.ServiceProvider.GetService<ILazyBuilder>();
            }
        }

        /// <summary>
        /// 使用lazy框架,将启动lazy框架,初始化模块
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void UseLazy(this IServiceProvider serviceProvider)
        {
            var moduleManager = serviceProvider.GetRequiredService<IModuleManager>();

            moduleManager.AllModule.ForEach_(r =>
            {
                r.Instance = serviceProvider.GetRequiredService(r.ModuleType) as LazyModule;

                r.Instance.OnInit();
            });
            moduleManager.AllModule.ForEach_(r =>
            {
                r.Instance.OnInited();
            });
        }
    }
}
