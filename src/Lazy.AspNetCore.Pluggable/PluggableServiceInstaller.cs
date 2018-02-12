using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Lazy.AspNetCore.Pluggable.Plugin;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lazy.AspNetCore.Pluggable
{
    static class PluggableServiceInstaller
    {
        public static void Installer(IServiceCollection services)
        {
            services.TryAddSingleton<IPluginManager, DefaultPluginManager>();
        }
    }
}
