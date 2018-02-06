using Lazy.Kernel.Module;
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
        /// <para><see cref="IModuleDependencyResolver"/></para>
        /// <para><see cref="IModuleManagaer"/></para>
        /// <para><see cref="Lazy.Kernel.Dependency.IIocManager"/></para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="startupOption"></param>
        /// <returns></returns>
        public static ILazyBuilder AddLazy(
            this IServiceCollection services,
            Assembly entryAssembly,
            Action<StartupOptions> startupOption = null)
        {
            services.Configure(startupOption ?? (r => { }));

            KernelServiceInstaller.Installer(services);
            StartupOptions options = new StartupOptions();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IConfigureOptions<StartupOptions>>().Configure(options);
                var moduleManager = scope.ServiceProvider.GetRequiredService<IModuleManagaer>();
                moduleManager.Init(entryAssembly, options);
            }

            return new LazyBuilder();
        }
    }
}
