using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Lazy.AspNetCore.Pluggable.Plugin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Lazy.AspNetCore.Pluggable.Mvc;
using Lazy.Kernel.Module;

namespace Lazy.AspNetCore.Pluggable
{
    static class PluggableServiceInstaller
    {
        public static void Installer(IServiceCollection services) {
            services.TryAddTransient(typeof(IModuleOptionProvider<>), typeof(DefaultModuleOptionProvider<>));
            services.TryAddSingleton<IPluginManager, DefaultPluginManager>();
            services.TryAddSingleton<IPluginRouterRegister, DefaultPluginRouterRegister>();
            services.TryAddSingleton<IPluginViewConfigure, DefaultPluginViewConfigure>();
        }
    }
}
