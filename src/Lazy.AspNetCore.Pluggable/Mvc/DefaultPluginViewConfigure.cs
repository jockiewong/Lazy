using System;
using System.Collections.Generic;
using System.Text;
using Lazy.Kernel.Module;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    public class DefaultPluginViewConfigure : IPluginViewConfigure
    {
        PluggableOptions _pluggableOptions;
        public DefaultPluginViewConfigure(IModuleOptionProvider<PluggableOptions> moduleOptionProvider)
        {
            _pluggableOptions = moduleOptionProvider.GetConfiguredOptions();
        }

        public void Configure(RazorViewEngineOptions options)
        {
            options.AreaViewLocationFormats.Add(_pluggableOptions.PluginSourceLocation.TrimEnd('/') + "/{2}/Views/{1}/{0}.cshtml");
        }
    }
}
